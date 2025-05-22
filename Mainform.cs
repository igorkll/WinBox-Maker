namespace WinBox_Maker
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
            this.Text = Program.version;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BuildProcess.Value = 30;
        }
    }
}
