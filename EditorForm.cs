namespace WinBox_Maker
{
    public partial class EditorForm : Form
    {
        WinBoxProject winBoxProject;

        public EditorForm(string path)
        {
            InitializeComponent();
            this.Text = $"{Program.version} - {this.Text} ({Path.GetFileName(Path.GetDirectoryName(path))})";

            winBoxProject = new WinBoxProject(path);
            string? err = winBoxProject.GetError();
            if (err == null)
            {
                MessageBox.Show(err, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Program.SwitchForm(this, new OpenProjectForm());
            }
        }

        private void StartBuild_Click(object sender, EventArgs e)
        {
            BuildProcess.Value = 30;
        }
    }
}
