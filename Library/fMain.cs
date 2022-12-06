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

        private void bCatalog_Click(object sender, EventArgs e)
        {

        }

        private void bMembers_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.Enabled = false;
            fMembers fm = new fMembers();
            fm.ShowDialog();
            this.Visible = true;
            this.Enabled = true;
        }

        private void fMain_Load(object sender, EventArgs e)
        {
            RegexController regexController = new RegexController();
            regexController.check();
        }
    }
}