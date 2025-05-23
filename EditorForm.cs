using System.IO;
using System.Xml.Linq;

namespace WinBox_Maker
{
    public partial class EditorForm : Form
    {
        WinBoxProject winBoxProject;

        public EditorForm(WinBoxProject winBoxProject)
        {
            InitializeComponent();
            this.Text = $"{Program.version} - {this.Text} ({winBoxProject.GetName()})";
            this.winBoxProject = winBoxProject;
            UpdateWindowsVersionsList();
            UpdateText();
        }

        private void StartBuild_Click(object sender, EventArgs e)
        {
            BuildProcess.Value = 30;
        }

        private void WindowsSelect_Click(object sender, EventArgs e)
        {
            string? name = winBoxProject.SelectResource("Windows image (*.iso)|*.iso");
            if (name != null)
            {
                winBoxProject.winBoxConfig.BaseWindowsImage = name;
                winBoxProject.SaveConfig();
                UpdateWindowsVersionsList();
                UpdateText();
            }
        }

        private void WindowsClear_Click(object sender, EventArgs e)
        {
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
                string[] windowsVersions = winBoxProject.GetWindowsVersionsInImage(winBoxProject.winBoxConfig.BaseWindowsImage);
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
                        winBoxProject.winBoxConfig.BaseWindowsVersion = WindowsVersionSelect.Items[0].ToString();
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
    }
}
