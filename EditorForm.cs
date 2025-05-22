namespace WinBox_Maker
{
    public partial class EditorForm : Form
    {
        public EditorForm(string path)
        {
            InitializeComponent();
            this.Text = $"{Program.version} - {this.Text} ({Path.GetFileName(Path.GetDirectoryName(path))})";
        }

        private void StartBuild_Click(object sender, EventArgs e)
        {
            BuildProcess.Value = 30;
        }
    }
}
