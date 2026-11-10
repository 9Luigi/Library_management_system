using Library.Presentation.Controllers;
using Library.Presentation.Theme;

namespace Library
{
	public partial class FormMain : Form
	{
		private FormMembers? _membersPage;
		private Button? _activeNav;

		public FormMain()
		{
			InitializeComponent();
			lblAppTitle.Font = AppTheme.TitleFont;
			foreach (var nav in new[] { bNavMembers, bNavCatalog, bTheme }) nav.Font = AppTheme.NavFont;

			AppTheme.Apply(this);
			ApplySidebarTheme();
			AppTheme.Changed += OnThemeChanged;
		}

		private void FMain_Load(object sender, EventArgs e)
		{
			ShowMembers();
		}

		private void BNavMembers_Click(object sender, EventArgs e) => ShowMembers();

		private void BTheme_Click(object sender, EventArgs e) => AppTheme.Toggle();

		private void OnThemeChanged(object? sender, EventArgs e) => ApplySidebarTheme();

		/// <summary>
		/// Shows the members page inside the content area (created once, recreated if it was closed).
		/// </summary>
		private void ShowMembers()
		{
			if (_membersPage == null || _membersPage.IsDisposed)
			{
				_membersPage = new FormMembers
				{
					TopLevel = false,
					FormBorderStyle = FormBorderStyle.None,
					Dock = DockStyle.Fill
				};
				contentHost.Controls.Add(_membersPage);
			}

			_membersPage.Show();
			_membersPage.BringToFront();
			SetActiveNav(bNavMembers);
		}

		private void SetActiveNav(Button nav)
		{
			_activeNav = nav;
			ApplySidebarTheme();
		}

		/// <summary>
		/// The sidebar is dark in both themes, so it is styled here instead of AppTheme.Apply.
		/// </summary>
		private void ApplySidebarTheme()
		{
			var p = AppTheme.Palette;
			sidebar.BackColor = p.Sidebar;
			lblAppTitle.ForeColor = Color.White;
			lblAppSubtitle.ForeColor = p.SidebarText;

			foreach (var nav in new[] { bNavMembers, bNavCatalog, bTheme })
			{
				bool active = nav == _activeNav;
				nav.FlatStyle = FlatStyle.Flat;
				nav.FlatAppearance.BorderSize = 0;
				nav.UseVisualStyleBackColor = false;
				nav.Cursor = Cursors.Hand;
				nav.BackColor = active ? p.SidebarActive : p.Sidebar;
				nav.ForeColor = active ? Color.White : p.SidebarText;
				nav.FlatAppearance.MouseOverBackColor = active ? p.SidebarActive : p.SidebarHover;
				nav.FlatAppearance.MouseDownBackColor = active ? p.SidebarActive : p.SidebarHover;
			}

			// Catalog is not implemented yet. A disabled flat button draws unreadable system-gray text
			// on the dark sidebar, so it stays enabled (no Click handler) and just looks muted.
			bNavCatalog.ForeColor = Color.FromArgb(107, 114, 128);
			bNavCatalog.Cursor = Cursors.Default;
			bNavCatalog.FlatAppearance.MouseOverBackColor = p.Sidebar;
			bNavCatalog.FlatAppearance.MouseDownBackColor = p.Sidebar;

			bTheme.Text = AppTheme.IsDark ? "Light theme" : "Dark theme";
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason != CloseReason.UserClosing) return;

			if (!MessageBoxHelper.ShowConfirmation("Are you sure you want to exit?", "Confirm Exit"))
			{
				e.Cancel = true;
				return;
			}

			AppTheme.Changed -= OnThemeChanged;
		}
	}
}
