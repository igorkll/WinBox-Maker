using System.IO;
using System.Xml.Linq;

namespace WinBox_Maker
{
    public partial class EditorForm : Form
    {
        const string defaultProcessName = "not busy";
        WinBoxProject winBoxProject;

        public EditorForm(WinBoxProject winBoxProject)
        {
            InitializeComponent();
            this.Text = $"{Program.version} - {this.Text} ({winBoxProject.GetName()})";
            this.winBoxProject = winBoxProject;
            ProcessName.Text = defaultProcessName;
            if (winBoxProject.NeedLoadWindows())
            {
                UpdateText();
                LoadWindowsTask();
            }
            else
            {
                UpdateWindowsVersionsList();
                UpdateText();
            }
        }

        async void LoadWindowsTask()
        {
            ProcessName.Text = "extracting install.wim";
            await winBoxProject.LoadWindowsImageAsync(ProcessValue);
            ProcessName.Text = defaultProcessName;
            ProcessValue.Value = 0;
            UpdateWindowsVersionsList();
            UpdateText();
        }

        private void StartBuild_Click(object sender, EventArgs e)
        {
            ProcessValue.Value = 30;
        }

        private void WindowsSelect_Click(object sender, EventArgs e)
        {
            string? name = winBoxProject.SelectResource("Windows image (*.iso)|*.iso");
            if (name != null)
            {
                winBoxProject.UnloadWindowsImage();
                winBoxProject.winBoxConfig.BaseWindowsImage = name;
                winBoxProject.SaveConfig();
                UpdateText();
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
            UpdateText();
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
            UpdateText();
        }

        private void WindowsVersionUpdate_Click(object sender, EventArgs e)
        {
            UpdateWindowsVersionsList();
            UpdateText();
        }

        void UpdateText()
        {
            WindowsName.Text = winBoxProject.winBoxConfig.BaseWindowsImage ?? "not selected";
            WindowsVersionSelect.Text = winBoxProject.winBoxConfig.BaseWindowsVersion ?? "";
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
                string[] windowsVersions = winBoxProject.GetWindowsVersions();
                WindowsVersionSelect.Items.Clear();
                bool exists = false;
                foreach (string item in windowsVersions)
                {
                    WindowsVersionSelect.Items.Add(item);
                    if (item.ToString() == winBoxProject.winBoxConfig.BaseWindowsVersion)
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    if (WindowsVersionSelect.Items.Count > 0)
                    {
                        bool findedPro = false;
                        foreach (string item in windowsVersions)
                        {
                            if (item.EndsWith("pro", StringComparison.OrdinalIgnoreCase))
                            {
                                winBoxProject.winBoxConfig.BaseWindowsVersion = item;
                                findedPro = true;
                            }
                        }

                        if (!findedPro)
                        {
                            winBoxProject.winBoxConfig.BaseWindowsVersion = windowsVersions[0];
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
            UpdateText();
        }
    }
}
