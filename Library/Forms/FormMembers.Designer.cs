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
				CancellationTokenSource.Cancel();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMembers));
			menuStrip1 = new MenuStrip();
			memberToolStripMenuItem = new ToolStripMenuItem();
			addMemberToolStripMenuItem = new ToolStripMenuItem();
			exitToolStripMenuItem = new ToolStripMenuItem();
			dataGridViewForMembers = new DataGridView();
			cmMember = new ContextMenuStrip(components);
			editToolStripMenuItem = new ToolStripMenuItem();
			deleteToolStripMenuItem = new ToolStripMenuItem();
			leToolStripMenuItem = new ToolStripMenuItem();
			seeLendedBooksForThisMemberToolStripMenuItem = new ToolStripMenuItem();
			pictureBox1 = new PictureBox();
			TbIINSearch = new TextBox();
			pbMembers = new ProgressBar();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewForMembers).BeginInit();
			cmMember.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// menuStrip1
			// 
			menuStrip1.Items.AddRange(new ToolStripItem[] { memberToolStripMenuItem, exitToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new Size(800, 24);
			menuStrip1.TabIndex = 1;
			menuStrip1.Text = "menuStrip1";
			// 
			// memberToolStripMenuItem
			// 
			memberToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addMemberToolStripMenuItem });
			memberToolStripMenuItem.Name = "memberToolStripMenuItem";
			memberToolStripMenuItem.Size = new Size(70, 20);
			memberToolStripMenuItem.Text = "Member..";
			// 
			// addMemberToolStripMenuItem
			// 
			addMemberToolStripMenuItem.Name = "addMemberToolStripMenuItem";
			addMemberToolStripMenuItem.Size = new Size(144, 22);
			addMemberToolStripMenuItem.Text = "Add member";
			addMemberToolStripMenuItem.Click += AddMemberToolStripMenuItem_Click;
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.Size = new Size(38, 20);
			exitToolStripMenuItem.Text = "Exit";
			exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
			// 
			// dataGridViewForMembers
			// 
			dataGridViewForMembers.AllowUserToAddRows = false;
			dataGridViewForMembers.AllowUserToDeleteRows = false;
			dataGridViewForMembers.AllowUserToResizeColumns = false;
			dataGridViewForMembers.AllowUserToResizeRows = false;
			dataGridViewForMembers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridViewForMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewForMembers.Location = new Point(12, 24);
			dataGridViewForMembers.MultiSelect = false;
			dataGridViewForMembers.Name = "dataGridViewForMembers";
			dataGridViewForMembers.ReadOnly = true;
			dataGridViewForMembers.RowTemplate.Height = 25;
			dataGridViewForMembers.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridViewForMembers.Size = new Size(572, 316);
			dataGridViewForMembers.TabIndex = 2;
			dataGridViewForMembers.CellMouseClick += View_CellMouseClick;
			// 
			// cmMember
			// 
			cmMember.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem, leToolStripMenuItem, seeLendedBooksForThisMemberToolStripMenuItem });
			cmMember.Name = "cmMember";
			cmMember.Size = new Size(231, 92);
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(230, 22);
			editToolStripMenuItem.Text = "Edit";
			editToolStripMenuItem.Click += EditToolStripMenuItem_Click;
			// 
			// deleteToolStripMenuItem
			// 
			deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			deleteToolStripMenuItem.Size = new Size(230, 22);
			deleteToolStripMenuItem.Text = "Delete";
			deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
			// 
			// leToolStripMenuItem
			// 
			leToolStripMenuItem.Name = "leToolStripMenuItem";
			leToolStripMenuItem.Size = new Size(230, 22);
			leToolStripMenuItem.Text = "Borrow a book";
			leToolStripMenuItem.Click += LeToolStripMenuItem_Click;
			// 
			// seeLendedBooksForThisMemberToolStripMenuItem
			// 
			seeLendedBooksForThisMemberToolStripMenuItem.Name = "seeLendedBooksForThisMemberToolStripMenuItem";
			seeLendedBooksForThisMemberToolStripMenuItem.Size = new Size(230, 22);
			seeLendedBooksForThisMemberToolStripMenuItem.Text = "Borrowed books/ return book";
			seeLendedBooksForThisMemberToolStripMenuItem.Click += SeeLendedBooksForThisMemberToolStripMenuItem_Click;
			// 
			// pictureBox1
			// 
			pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
			pictureBox1.Location = new Point(590, 24);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(198, 316);
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 3;
			pictureBox1.TabStop = false;
			// 
			// TbIINSearch
			// 
			TbIINSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			TbIINSearch.Location = new Point(326, 375);
			TbIINSearch.Name = "TbIINSearch";
			TbIINSearch.Size = new Size(148, 23);
			TbIINSearch.TabIndex = 4;
			TbIINSearch.Text = "Enter IIN";
			TbIINSearch.TextAlign = HorizontalAlignment.Center;
			TbIINSearch.Click += TbIINSearch_Click;
			TbIINSearch.TextChanged += TbIINSearch_TextChanged;
			// 
			// pbMembers
			// 
			pbMembers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pbMembers.Location = new Point(12, 346);
			pbMembers.Name = "pbMembers";
			pbMembers.Size = new Size(776, 23);
			pbMembers.Step = 1;
			pbMembers.TabIndex = 6;
			// 
			// FMembers
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 439);
			Controls.Add(pbMembers);
			Controls.Add(TbIINSearch);
			Controls.Add(pictureBox1);
			Controls.Add(dataGridViewForMembers);
			Controls.Add(menuStrip1);
			MainMenuStrip = menuStrip1;
			Name = "FMembers";
			Text = "Members";
			FormClosing += FMembers_FormClosing;
			Load += FMembers_Load;
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewForMembers).EndInit();
			cmMember.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private MenuStrip menuStrip1;
        private ToolStripMenuItem memberToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private DataGridView dataGridViewForMembers;
        private PictureBox pictureBox1;
        private ToolStripMenuItem addMemberToolStripMenuItem;
        private TextBox TbIINSearch;
        private ProgressBar pbMembers;
        private ContextMenuStrip cmMember;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem leToolStripMenuItem;
        private ToolStripMenuItem seeLendedBooksForThisMemberToolStripMenuItem;
    }
}