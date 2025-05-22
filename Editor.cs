namespace WinBox_Maker
{
    public partial class Editor : Form
    {
        public Editor()
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
