using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace WinBox_Maker
{
    public partial class EditorForm : Form
    {
        const string defaultProcessName = "not busy";
        WinBoxProject winBoxProject;
        WindowsDescription[]? windowsDescriptions;

        public EditorForm(WinBoxProject winBoxProject)
        {
            InitializeComponent();
            this.Text = $"{Program.version} - {this.Text} ({winBoxProject.GetName()})";
            this.winBoxProject = winBoxProject;
            UnlockForm();
            if (winBoxProject.NeedLoadWindows())
            {
                UpdateGui();
                LoadWindowsTask();
            }
            else
            {
                UpdateWindowsVersionsList();
                UpdateGui();
            }
        }

        void UnlockForm()
        {
            ProcessName.Text = defaultProcessName;
            ProcessValue.Value = 0;
            foreach (Control control in this.Controls)
            {
                control.Enabled = true;
            }
            UpdateGui();
        }

        void LockForm()
        {
            foreach (Control control in this.Controls)
            {
                if (control != ProcessValue && !(control is Label))
                {
                    control.Enabled = false;
                }
            }
        }

        async void LoadWindowsTask()
        {
            LockForm();
            await winBoxProject.LoadWindowsImageAsync(ProcessName, ProcessValue);
            UnlockForm();
            UpdateWindowsVersionsList();
            UpdateGui();
        }

        private async void ExportIsoInstaller_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox installer (*.iso)|*.iso";
                saveFileDialog.Title = $"Save you WinBox installer ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "iso";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName;
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    await winBoxProject.BuildIsoAsync(ProcessName, ProcessValue, saveFileDialog.FileName);
                }
            }
            UnlockForm();
        }

        private async void WindowsSelect_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(ProcessName, ProcessValue, "Windows image (*.iso)|*.iso");
            UnlockForm();
            if (name != null)
            {
                winBoxProject.UnloadWindowsImage();
                winBoxProject.winBoxConfig.BaseWindowsImage = name;
                winBoxProject.SaveConfig();
                UpdateGui();
                LoadWindowsTask();
            }
        }

        private void WindowsClear_Click(object sender, EventArgs e)
        {
            winBoxProject.UnloadWindowsImage();
            WindowsVersionSelect.Items.Clear();
            winBoxProject.winBoxConfig.BaseWindowsVersion = null;
            winBoxProject.winBoxConfig.BaseWindowsImage = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void WindowsVersionSelect_TextChanged(object sender, EventArgs e)
        {
            if (winBoxProject.winBoxConfig.BaseWindowsImage == null)
            {
                WindowsVersionSelect.Text = null;
                return;
            }

            winBoxProject.winBoxConfig.BaseWindowsVersion = WindowsVersionSelect.Text;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void WindowsVersionUpdate_Click(object sender, EventArgs e)
        {
            UpdateWindowsVersionsList();
            UpdateGui();
        }

        void UpdateGui()
        {
            WindowsName.Text = winBoxProject.winBoxConfig.BaseWindowsImage ?? "not selected";
            WindowsVersionSelect.Text = winBoxProject.winBoxConfig.BaseWindowsVersion ?? "";

            WinboxName.Text = winBoxProject.winBoxConfig.WinboxName;
            WinboxDescription.Text = winBoxProject.winBoxConfig.WinboxDescription;

            WindowsDescription.Text = "";
            if (windowsDescriptions != null && winBoxProject.winBoxConfig.BaseWindowsVersion != null)
            {
                foreach (WindowsDescription windowsDescription in windowsDescriptions)
                {
                    if (windowsDescription.name == winBoxProject.winBoxConfig.BaseWindowsVersion)
                    {
                        WindowsDescription.Text = windowsDescription.description;
                        break;
                    }
                }
            }

            bool canExport = true;
            if (winBoxProject.winBoxConfig.BaseWindowsImage == null || winBoxProject.winBoxConfig.BaseWindowsVersion == null)
            {
                canExport = false;
            }
            if (canExport)
            {
                bool exists = false;
                WindowsDescription[] localWindowsDescriptions = winBoxProject.GetWindowsDescriptions();
                foreach (WindowsDescription item in localWindowsDescriptions)
                {
                    if (item.name == winBoxProject.winBoxConfig.BaseWindowsVersion)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                {
                    canExport = false;
                }
            }

            ExportIsoInstaller.Enabled = canExport;
        }

        void UpdateWindowsVersionsList()
        {
            if (winBoxProject.winBoxConfig.BaseWindowsImage == null)
            {
                winBoxProject.winBoxConfig.BaseWindowsVersion = null;
                winBoxProject.SaveConfig();
                return;
            }

            try
            {
                windowsDescriptions = winBoxProject.GetWindowsDescriptions();
                WindowsVersionSelect.Items.Clear();
                bool exists = false;
                foreach (WindowsDescription item in windowsDescriptions)
                {
                    WindowsVersionSelect.Items.Add(item.name);
                    if (item.name == winBoxProject.winBoxConfig.BaseWindowsVersion)
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    if (WindowsVersionSelect.Items.Count > 0)
                    {
                        bool findedPro = false;
                        foreach (WindowsDescription item in windowsDescriptions)
                        {
                            if (item.name.EndsWith("pro", StringComparison.OrdinalIgnoreCase))
                            {
                                winBoxProject.winBoxConfig.BaseWindowsVersion = item.name;
                                findedPro = true;
                            }
                        }

                        if (!findedPro)
                        {
                            winBoxProject.winBoxConfig.BaseWindowsVersion = windowsDescriptions[0].name;
                        }
                    }
                    else
                    {
                        winBoxProject.winBoxConfig.BaseWindowsVersion = null;
                    }
                }

                winBoxProject.SaveConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"couldn't read the list of versions from the image: {ex}", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WindowsVersionClear_Click(object sender, EventArgs e)
        {
            WindowsVersionSelect.Items.Clear();
            winBoxProject.winBoxConfig.BaseWindowsVersion = null;
            winBoxProject.SaveConfig();
            UpdateGui();
        }

        private void WinboxName_TextChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.WinboxName = WinboxName.Text;
            winBoxProject.SaveConfig();
        }

        private void WinboxDescription_TextChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.WinboxDescription = WinboxDescription.Text;
            winBoxProject.SaveConfig();
        }
    }
}
