namespace WinBox_Maker
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
            this.Text = Program.version + " - " + this.Text;
        }

        private void StartBuild_Click(object sender, EventArgs e)
        {
            BuildProcess.Value = 30;
        }
    }
}
