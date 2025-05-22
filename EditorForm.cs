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

        void UpdateText()
        {
            WindowsName.Text = winBoxProject.winBoxConfig.BaseWindowsImage;
        }
    }
}
