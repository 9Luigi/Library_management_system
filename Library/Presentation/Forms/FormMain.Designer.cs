namespace Library
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			sidebar = new Panel();
			bTheme = new Button();
			bNavCatalog = new Button();
			bNavMembers = new Button();
			lblAppSubtitle = new Label();
			lblAppTitle = new Label();
			contentHost = new Panel();
			sidebar.SuspendLayout();
			SuspendLayout();
			//
			// sidebar
			//
			sidebar.Controls.Add(bTheme);
			sidebar.Controls.Add(bNavCatalog);
			sidebar.Controls.Add(bNavMembers);
			sidebar.Controls.Add(lblAppSubtitle);
			sidebar.Controls.Add(lblAppTitle);
			sidebar.Dock = DockStyle.Left;
			sidebar.Location = new Point(0, 0);
			sidebar.Name = "sidebar";
			sidebar.Size = new Size(220, 720);
			sidebar.TabIndex = 0;
			sidebar.Tag = "theme:skip";
			//
			// bTheme
			//
			bTheme.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			bTheme.Location = new Point(12, 664);
			bTheme.Name = "bTheme";
			bTheme.Padding = new Padding(12, 0, 0, 0);
			bTheme.Size = new Size(196, 44);
			bTheme.TabIndex = 3;
			bTheme.Text = "Dark theme";
			bTheme.TextAlign = ContentAlignment.MiddleLeft;
			bTheme.Click += BTheme_Click;
			//
			// bNavCatalog
			//
			bNavCatalog.Location = new Point(12, 152);
			bNavCatalog.Name = "bNavCatalog";
			bNavCatalog.Padding = new Padding(12, 0, 0, 0);
			bNavCatalog.Size = new Size(196, 44);
			bNavCatalog.TabIndex = 2;
			bNavCatalog.TabStop = false;
			bNavCatalog.Text = "Catalog  (soon)";
			bNavCatalog.TextAlign = ContentAlignment.MiddleLeft;
			//
			// bNavMembers
			//
			bNavMembers.Location = new Point(12, 100);
			bNavMembers.Name = "bNavMembers";
			bNavMembers.Padding = new Padding(12, 0, 0, 0);
			bNavMembers.Size = new Size(196, 44);
			bNavMembers.TabIndex = 1;
			bNavMembers.Text = "Members";
			bNavMembers.TextAlign = ContentAlignment.MiddleLeft;
			bNavMembers.Click += BNavMembers_Click;
			//
			// lblAppSubtitle
			//
			lblAppSubtitle.AutoSize = true;
			lblAppSubtitle.Location = new Point(22, 58);
			lblAppSubtitle.Name = "lblAppSubtitle";
			lblAppSubtitle.Size = new Size(147, 20);
			lblAppSubtitle.TabIndex = 4;
			lblAppSubtitle.Text = "Management system";
			//
			// lblAppTitle
			//
			lblAppTitle.AutoSize = true;
			lblAppTitle.Location = new Point(20, 20);
			lblAppTitle.Name = "lblAppTitle";
			lblAppTitle.Size = new Size(84, 37);
			lblAppTitle.TabIndex = 5;
			lblAppTitle.Text = "Library";
			//
			// contentHost
			//
			contentHost.Dock = DockStyle.Fill;
			contentHost.Location = new Point(220, 0);
			contentHost.Name = "contentHost";
			contentHost.Size = new Size(1080, 720);
			contentHost.TabIndex = 1;
			//
			// FormMain
			//
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1300, 720);
			Controls.Add(contentHost);
			Controls.Add(sidebar);
			MinimumSize = new Size(1100, 680);
			Name = "FormMain";
			StartPosition = FormStartPosition.CenterScreen;
			Text = "Library";
			FormClosing += FormMain_FormClosing;
			Load += FMain_Load;
			sidebar.ResumeLayout(false);
			sidebar.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private Panel sidebar;
		private Label lblAppTitle;
		private Label lblAppSubtitle;
		private Button bNavMembers;
		private Button bNavCatalog;
		private Button bTheme;
		private Panel contentHost;
    }
}
