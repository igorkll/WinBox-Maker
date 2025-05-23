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
            UpdateText();
            WindowsVersionSelect.Text = winBoxProject.winBoxConfig.BaseWindowsVersion;
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
                UpdateText();
            }
        }

        private void WindowsClear_Click(object sender, EventArgs e)
        {
            if (winBoxProject.winBoxConfig.BaseWindowsImage != null)
            {
                winBoxProject.winBoxConfig.BaseWindowsImage = null;
                winBoxProject.SaveConfig();
                UpdateText();
            }
        }

        void UpdateText()
        {
            WindowsName.Text = winBoxProject.winBoxConfig.BaseWindowsImage ?? "not selected";
        }

        private void WindowsVersionDetect_Click(object sender, EventArgs e)
        {
            if (winBoxProject.winBoxConfig.BaseWindowsImage != null)
            {
                try
                {
                    string[] windowsVersions = winBoxProject.GetWindowsVersionsInImage(winBoxProject.winBoxConfig.BaseWindowsImage);
                    WindowsVersionSelect.Items.Clear();
                    foreach (string item in windowsVersions)
                    {
                        WindowsVersionSelect.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"couldn't read the list of versions from the image: {ex}", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("first, select the windows image", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WindowsVersionClear_Click(object sender, EventArgs e)
        {
            WindowsVersionSelect.Items.Clear();
        }

        private void WindowsVersionSelect_TextChanged(object sender, EventArgs e)
        {
            winBoxProject.winBoxConfig.BaseWindowsVersion = WindowsVersionSelect.Text;
            winBoxProject.SaveConfig();
            UpdateText();
        }
    }
}
