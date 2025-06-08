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
            this.Text = $"{WinBox_Maker.Program.version} - {this.Text} ({winBoxProject.GetName()})";
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
                if (control.Name != "LICENSE" && control.Name != "README" && !(control is ProgressBar) && !(control is Label) && !(control is PictureBox))
                {
                    control.Enabled = false;
                }
            }
        }

        async void LoadWindowsTask()
        {
            LockForm();
            await winBoxProject.LoadWindowsImageAsync(UpdateProcessName, UpdateProcessValue);
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
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildIsoAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
                }
            }
            UnlockForm();
        }

        private async void ExportInstallWim_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox (*.wim)|*.wim";
                saveFileDialog.Title = $"Save you WinBox install.wim ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "iso";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName;
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildWimAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
                }
            }
            UnlockForm();
        }

        private async void ExportImgPartition_Click(object sender, EventArgs e)
        {
            LockForm();
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.InitialDirectory = winBoxProject.buildDirectoryPath;
                saveFileDialog.Filter = "WinBox installed partition (*.img)|*.img";
                saveFileDialog.Title = $"Save you WinBox installed .img partition ({winBoxProject.winBoxConfig.WinboxName})";
                saveFileDialog.DefaultExt = "iso";
                saveFileDialog.FileName = winBoxProject.winBoxConfig.WinboxName + " (partition)";
                saveFileDialog.AddExtension = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    WindowsDescription windowsDescription = new WindowsDescription
                    {
                        name = winBoxProject.winBoxConfig.WinboxName,
                        description = winBoxProject.winBoxConfig.WinboxDescription
                    };
                    await winBoxProject.BuildImgPartitionAsync(UpdateProcessName, UpdateProcessValue, saveFileDialog.FileName, windowsDescription);
                }
            }
            UnlockForm();
        }

        private async void WindowsSelect_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Windows image (*.iso)|*.iso", winBoxProject.bigResourcesDirectoryPath, false);
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
            UpdateGuiWithoutWindowsVersion();
        }

        private void WindowsVersionUpdate_Click(object sender, EventArgs e)
        {
            UpdateWindowsVersionsList();
            UpdateGui();
        }

        void UpdateGuiWithoutWindowsVersion()
        {
            WindowsName.Text = winBoxProject.winBoxConfig.BaseWindowsImage ?? "not selected";

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
            ExportInstallWim.Enabled = canExport;
            ExportImgPartition.Enabled = canExport;
        }

        void UpdateGui()
        {
            WindowsVersionSelect.Text = winBoxProject.winBoxConfig.BaseWindowsVersion ?? "";
            OemKey.Text = winBoxProject.winBoxConfig.OemKey ?? "";
            UseOemKey.CheckState = winBoxProject.winBoxConfig.UseOemKey == true ? CheckState.Checked : CheckState.Unchecked;
            ProgramName.Text = winBoxProject.winBoxConfig.ProgramName ?? "not selected";
            ProgramArgs.Text = winBoxProject.winBoxConfig.ProgramArgs ?? "";
            ProgramAsAdmin.CheckState = winBoxProject.winBoxConfig.ProgramAsAdmin == true ? CheckState.Checked : CheckState.Unchecked;
            UpdateGuiWithoutWindowsVersion();
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

        private void OemKey_TextChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.OemKey = OemKey.Text;
            winBoxProject.SaveConfig();
        }

        private void UseOemKey_CheckedChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.UseOemKey = UseOemKey.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void ProgramArgs_TextChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.ProgramArgs = ProgramArgs.Text;
            winBoxProject.SaveConfig();
        }

        private void ProgramAsAdmin_CheckedChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.ProgramAsAdmin = ProgramAsAdmin.CheckState == CheckState.Checked;
            winBoxProject.SaveConfig();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            WinBox_Maker.Program.OpenWebPage(WinBox_Maker.Program.logichubUrl + "#winbox");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            WinBox_Maker.Program.OpenWebPage(WinBox_Maker.Program.logichubUrl);
        }

        private void UpdateProcessName(string text)
        {
            ProcessName.Text = text;
        }

        private void UpdateProcessValue(int Value)
        {
            ProcessValue.Value = Value;
        }

        private void back_Click(object sender, EventArgs e)
        {
            WinBox_Maker.Program.SwitchForm(this, WinBox_Maker.Program.openProjectForm);
        }

        private void README_Click(object sender, EventArgs e)
        {
            Form form = new TextViewer("README.md");
            form.Show();
        }

        private void LICENSE_Click(object sender, EventArgs e)
        {
            Form form = new TextViewer("LICENSE.txt");
            form.Show();
        }

        private async void AppSelect_Click(object sender, EventArgs e)
        {
            LockForm();
            string? name = await winBoxProject.SelectResourceAsync(UpdateProcessName, UpdateProcessValue, "Executable files (*.exe;*.bat)|*.exe;*.bat|All files (*.*)|*.*", Path.Combine(winBoxProject.resourcesDirectoryPath, "program"), true);
            UnlockForm();
            if (name != null)
            {
                winBoxProject.winBoxConfig.ProgramName = name;
            }
            winBoxProject.SaveConfig();
        }

        private async void AppClear_Click(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.ProgramName = null;
            winBoxProject.SaveConfig();
        }
    }
}
