namespace Library
{
	public partial class FMain : Form
	{
		public FMain()
		{
			InitializeComponent();
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = true;
			FM = new FMembers();
		}
		internal FMembers FM { get; private set; }
		private void bCatalog_Click(object sender, EventArgs e)
		{

		}
		private void bMembers_Click(object sender, EventArgs e)
		{
			ToggleMainFornVisibility(false);
			FM.ShowDialog();
			ToggleMainFornVisibility(true);
		}

		private void FMain_Load(object sender, EventArgs e)
		{
			
		}
		private void ToggleMainFornVisibility(bool isVisible)
		{
			this.Visible = isVisible;
			this.Enabled = isVisible;
		}
	}
}