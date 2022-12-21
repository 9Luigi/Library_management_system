using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Library
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
            
        }
        internal FMembers fm { get; private set; }
        private void bCatalog_Click(object sender, EventArgs e)
        {

        }

        private void bMembers_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Enabled = false;
            fm.ShowDialog();
            this.Visible = true;
            this.Enabled = true;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            fm = new FMembers();
        }
    }
}