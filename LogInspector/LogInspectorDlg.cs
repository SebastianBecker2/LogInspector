namespace LogInspector
{
    public partial class LogInspectorDlg : Form
    {
        public LogInspectorDlg()
        {
            InitializeComponent();
        }

        public void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
