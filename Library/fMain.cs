using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Library
{
	public partial class FMain : Form
	{
		public FMain()
		{
			InitializeComponent();
			FM = new FMembers();
		}
		internal FMembers FM { get; private set; }
		private void bCatalog_Click(object sender, EventArgs e)
		{

		}
		private void bMembers_Click(object sender, EventArgs e)
		{
			this.Visible = false;
			this.Enabled = false;
			FM.ShowDialog();
			this.Visible = true;
			this.Enabled = true;
		}

		private void FMain_Load(object sender, EventArgs e)
		{
			this.FormBorderStyle = FormBorderStyle.FixedSingle; 
			this.MaximizeBox = false; 
			this.Size = new Size(640, 480); 
		}
	}
}