namespace Library
{
    partial class FormMembers
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			menuStrip1 = new MenuStrip();
			memberToolStripMenuItem = new ToolStripMenuItem();
			addMemberToolStripMenuItem = new ToolStripMenuItem();
			exitToolStripMenuItem = new ToolStripMenuItem();
			dataGridViewForMembers = new DataGridView();
			cmMember = new ContextMenuStrip(components);
			editToolStripMenuItem = new ToolStripMenuItem();
			deleteToolStripMenuItem = new ToolStripMenuItem();
			BorrowBookToolStripMenuItem = new ToolStripMenuItem();
			ReturnBookToolStripMenuItem = new ToolStripMenuItem();
			pictureBoxMember = new PictureBox();
			TbIINSearch = new TextBox();
			bSearch = new Button();
			bClearSearch = new Button();
			lblRowsPerPage = new Label();
			cbPageSize = new ComboBox();
			lblRange = new Label();
			bPrevPage = new Button();
			lblPageInfo = new Label();
			bNextPage = new Button();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewForMembers).BeginInit();
			cmMember.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBoxMember).BeginInit();
			SuspendLayout();
			// 
			// menuStrip1
			// 
			menuStrip1.ImageScalingSize = new Size(20, 20);
			menuStrip1.Items.AddRange(new ToolStripItem[] { memberToolStripMenuItem, exitToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Padding = new Padding(7, 3, 0, 3);
			menuStrip1.Size = new Size(1079, 30);
			menuStrip1.TabIndex = 1;
			menuStrip1.Text = "mainMenu";
			// 
			// memberToolStripMenuItem
			// 
			memberToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addMemberToolStripMenuItem });
			memberToolStripMenuItem.Name = "memberToolStripMenuItem";
			memberToolStripMenuItem.Size = new Size(85, 24);
			memberToolStripMenuItem.Text = "Member..";
			// 
			// addMemberToolStripMenuItem
			// 
			addMemberToolStripMenuItem.Name = "addMemberToolStripMenuItem";
			addMemberToolStripMenuItem.Size = new Size(180, 26);
			addMemberToolStripMenuItem.Text = "Add member";
			addMemberToolStripMenuItem.Click += AddMemberToolStripMenuItem_Click;
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.Size = new Size(47, 24);
			exitToolStripMenuItem.Text = "Exit";
			exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
			// 
			// dataGridViewForMembers
			// 
			dataGridViewForMembers.AllowUserToAddRows = false;
			dataGridViewForMembers.AllowUserToDeleteRows = false;
			dataGridViewForMembers.AllowUserToOrderColumns = true;
			dataGridViewForMembers.AllowUserToResizeColumns = false;
			dataGridViewForMembers.AllowUserToResizeRows = false;
			dataGridViewForMembers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridViewForMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewForMembers.Location = new Point(14, 80);
			dataGridViewForMembers.Margin = new Padding(3, 4, 3, 4);
			dataGridViewForMembers.MultiSelect = false;
			dataGridViewForMembers.Name = "dataGridViewForMembers";
			dataGridViewForMembers.ReadOnly = true;
			dataGridViewForMembers.RowHeadersWidth = 51;
			dataGridViewForMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridViewForMembers.Size = new Size(770, 540);
			dataGridViewForMembers.TabIndex = 2;
			dataGridViewForMembers.CellMouseClick += View_CellMouseClick;
			dataGridViewForMembers.CellMouseDoubleClick += DataGridViewForMembers_CellMouseDoubleClick;
			dataGridViewForMembers.SelectionChanged += DataGridViewForMembers_SelectionChanged;
			// 
			// cmMember
			// 
			cmMember.ImageScalingSize = new Size(20, 20);
			cmMember.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem, BorrowBookToolStripMenuItem, ReturnBookToolStripMenuItem });
			cmMember.Name = "cmMember";
			cmMember.Size = new Size(177, 100);
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(176, 24);
			editToolStripMenuItem.Text = "Edit";
			editToolStripMenuItem.Click += EditToolStripMenuItem_Click;
			// 
			// deleteToolStripMenuItem
			// 
			deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			deleteToolStripMenuItem.Size = new Size(176, 24);
			deleteToolStripMenuItem.Text = "Delete";
			deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
			// 
			// BorrowBookToolStripMenuItem
			// 
			BorrowBookToolStripMenuItem.Name = "BorrowBookToolStripMenuItem";
			BorrowBookToolStripMenuItem.Size = new Size(176, 24);
			BorrowBookToolStripMenuItem.Text = "Borrow a book";
			BorrowBookToolStripMenuItem.Click += BorrowBookToolStripMenuItem_Click;
			// 
			// ReturnBookToolStripMenuItem
			// 
			ReturnBookToolStripMenuItem.Name = "ReturnBookToolStripMenuItem";
			ReturnBookToolStripMenuItem.Size = new Size(176, 24);
			ReturnBookToolStripMenuItem.Text = "Return book";
			ReturnBookToolStripMenuItem.Click += ReturnBookToolStripMenuItem_Click;
			// 
			// pictureBoxMember
			// 
			pictureBoxMember.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			pictureBoxMember.Image = Properties.Resources.NoImage;
			pictureBoxMember.Location = new Point(798, 80);
			pictureBoxMember.Margin = new Padding(3, 4, 3, 4);
			pictureBoxMember.Name = "pictureBoxMember";
			pictureBoxMember.Size = new Size(267, 356);
			pictureBoxMember.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBoxMember.TabIndex = 3;
			pictureBoxMember.TabStop = false;
			// 
			// TbIINSearch
			// 
			TbIINSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			TbIINSearch.Location = new Point(14, 42);
			TbIINSearch.Margin = new Padding(3, 4, 3, 4);
			TbIINSearch.MaxLength = 12;
			TbIINSearch.Name = "TbIINSearch";
			TbIINSearch.PlaceholderText = "Enter full IIN (12 digits)";
			TbIINSearch.Size = new Size(550, 27);
			TbIINSearch.TabIndex = 4;
			TbIINSearch.TextChanged += TbIINSearch_TextChanged;
			TbIINSearch.KeyDown += TbIINSearch_KeyDown;
			TbIINSearch.KeyPress += TbIINSearch_KeyPress;
			//
			// bSearch
			//
			bSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			bSearch.Location = new Point(572, 38);
			bSearch.Name = "bSearch";
			bSearch.Size = new Size(102, 34);
			bSearch.TabIndex = 11;
			bSearch.Text = "Search";
			bSearch.Click += BSearch_Click;
			//
			// bClearSearch
			//
			bClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			bClearSearch.Location = new Point(682, 38);
			bClearSearch.Name = "bClearSearch";
			bClearSearch.Size = new Size(102, 34);
			bClearSearch.TabIndex = 12;
			bClearSearch.Text = "Clear";
			bClearSearch.Click += BClearSearch_Click;
			//
			// lblRowsPerPage
			//
			lblRowsPerPage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			lblRowsPerPage.AutoSize = true;
			lblRowsPerPage.Location = new Point(14, 637);
			lblRowsPerPage.Name = "lblRowsPerPage";
			lblRowsPerPage.Size = new Size(99, 20);
			lblRowsPerPage.TabIndex = 5;
			lblRowsPerPage.Text = "Rows per page";
			//
			// cbPageSize
			//
			cbPageSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			cbPageSize.DropDownStyle = ComboBoxStyle.DropDownList;
			cbPageSize.Location = new Point(122, 633);
			cbPageSize.Name = "cbPageSize";
			cbPageSize.Size = new Size(70, 28);
			cbPageSize.TabIndex = 6;
			cbPageSize.SelectedIndexChanged += CbPageSize_SelectedIndexChanged;
			//
			// lblRange
			//
			lblRange.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			lblRange.AutoSize = true;
			lblRange.Location = new Point(206, 637);
			lblRange.Name = "lblRange";
			lblRange.Size = new Size(0, 20);
			lblRange.TabIndex = 7;
			//
			// bPrevPage
			//
			bPrevPage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			bPrevPage.Location = new Point(444, 630);
			bPrevPage.Name = "bPrevPage";
			bPrevPage.Size = new Size(100, 34);
			bPrevPage.TabIndex = 8;
			bPrevPage.Text = "‹ Prev";
			bPrevPage.Click += BPrevPage_Click;
			//
			// lblPageInfo
			//
			lblPageInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			lblPageInfo.Location = new Point(548, 630);
			lblPageInfo.Name = "lblPageInfo";
			lblPageInfo.Size = new Size(132, 34);
			lblPageInfo.TabIndex = 9;
			lblPageInfo.Text = "Page 1 of 1";
			lblPageInfo.TextAlign = ContentAlignment.MiddleCenter;
			//
			// bNextPage
			//
			bNextPage.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			bNextPage.Location = new Point(684, 630);
			bNextPage.Name = "bNextPage";
			bNextPage.Size = new Size(100, 34);
			bNextPage.TabIndex = 10;
			bNextPage.Text = "Next ›";
			bNextPage.Click += BNextPage_Click;
			//
			// FormMembers
			//
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1079, 668);
			Controls.Add(bNextPage);
			Controls.Add(lblPageInfo);
			Controls.Add(bPrevPage);
			Controls.Add(lblRange);
			Controls.Add(cbPageSize);
			Controls.Add(lblRowsPerPage);
			Controls.Add(bClearSearch);
			Controls.Add(bSearch);
			Controls.Add(TbIINSearch);
			Controls.Add(pictureBoxMember);
			Controls.Add(dataGridViewForMembers);
			Controls.Add(menuStrip1);
			MainMenuStrip = menuStrip1;
			Margin = new Padding(3, 4, 3, 4);
			Name = "FormMembers";
			Text = "Members";
			FormClosing += FMembers_FormClosing;
			Load += FMembers_Load;
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewForMembers).EndInit();
			cmMember.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)pictureBoxMember).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private MenuStrip menuStrip1;
        private ToolStripMenuItem memberToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private DataGridView dataGridViewForMembers;
        private PictureBox pictureBoxMember;
        private ToolStripMenuItem addMemberToolStripMenuItem;
        private TextBox TbIINSearch;
		private Button bSearch;
		private Button bClearSearch;
		private Label lblRowsPerPage;
		private ComboBox cbPageSize;
		private Label lblRange;
		private Button bPrevPage;
		private Label lblPageInfo;
		private Button bNextPage;
        private ContextMenuStrip cmMember;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem BorrowBookToolStripMenuItem;
        private ToolStripMenuItem ReturnBookToolStripMenuItem;
	}
}