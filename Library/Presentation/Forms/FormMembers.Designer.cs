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
			BorrowBookToolStripMenuItem = new ToolStripMenuItem();
			ReturnBookToolStripMenuItem = new ToolStripMenuItem();
			pictureBoxMember = new PictureBox();
			TbIINSearch = new TextBox();
			pbMembers = new ProgressBar();
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
			menuStrip1.Size = new Size(944, 24);
			menuStrip1.TabIndex = 1;
			menuStrip1.Text = "mainMenu";
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
			dataGridViewForMembers.RowHeadersWidth = 51;
			dataGridViewForMembers.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridViewForMembers.Size = new Size(674, 360);
			dataGridViewForMembers.TabIndex = 2;
			dataGridViewForMembers.CellMouseClick += View_CellMouseClick;
			dataGridViewForMembers.CellMouseDoubleClick += DataGridViewForMembers_CellMouseDoubleClick;
			dataGridViewForMembers.CurrentCellChanged += DataGridViewForMembers_CurrentCellChanged;
			// 
			// cmMember
			// 
			cmMember.ImageScalingSize = new Size(20, 20);
			cmMember.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem, BorrowBookToolStripMenuItem, ReturnBookToolStripMenuItem });
			cmMember.Name = "cmMember";
			cmMember.Size = new Size(152, 92);
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(151, 22);
			editToolStripMenuItem.Text = "Edit";
			editToolStripMenuItem.Click += EditToolStripMenuItem_Click;
			// 
			// deleteToolStripMenuItem
			// 
			deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			deleteToolStripMenuItem.Size = new Size(151, 22);
			deleteToolStripMenuItem.Text = "Delete";
			deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
			// 
			// BorrowBookToolStripMenuItem
			// 
			BorrowBookToolStripMenuItem.Name = "BorrowBookToolStripMenuItem";
			BorrowBookToolStripMenuItem.Size = new Size(151, 22);
			BorrowBookToolStripMenuItem.Text = "Borrow a book";
			BorrowBookToolStripMenuItem.Click += BorrowBookToolStripMenuItem_Click;
			// 
			// ReturnBookToolStripMenuItem
			// 
			ReturnBookToolStripMenuItem.Name = "ReturnBookToolStripMenuItem";
			ReturnBookToolStripMenuItem.Size = new Size(151, 22);
			ReturnBookToolStripMenuItem.Text = "Return book";
			ReturnBookToolStripMenuItem.Click += ReturnBookToolStripMenuItem_Click;
			// 
			// pictureBoxMember
			// 
			pictureBoxMember.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			pictureBoxMember.Image = (Image)resources.GetObject("pictureBoxMember.Image");
			pictureBoxMember.Location = new Point(692, 24);
			pictureBoxMember.Name = "pictureBoxMember";
			pictureBoxMember.Size = new Size(240, 360);
			pictureBoxMember.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBoxMember.TabIndex = 3;
			pictureBoxMember.TabStop = false;
			// 
			// TbIINSearch
			// 
			TbIINSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			TbIINSearch.Location = new Point(326, 437);
			TbIINSearch.Name = "TbIINSearch";
			TbIINSearch.Size = new Size(292, 23);
			TbIINSearch.TabIndex = 4;
			TbIINSearch.Text = "Enter IIN";
			TbIINSearch.TextAlign = HorizontalAlignment.Center;
			TbIINSearch.Click += TbIINSearch_Click;
			TbIINSearch.TextChanged += TbIINSearch_TextChanged;
			// 
			// pbMembers
			// 
			pbMembers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pbMembers.Location = new Point(12, 390);
			pbMembers.Name = "pbMembers";
			pbMembers.Size = new Size(920, 23);
			pbMembers.Step = 1;
			pbMembers.TabIndex = 6;
			// 
			// FormMembers
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(944, 501);
			Controls.Add(pbMembers);
			Controls.Add(TbIINSearch);
			Controls.Add(pictureBoxMember);
			Controls.Add(dataGridViewForMembers);
			Controls.Add(menuStrip1);
			MainMenuStrip = menuStrip1;
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
        private ProgressBar pbMembers;
        private ContextMenuStrip cmMember;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem BorrowBookToolStripMenuItem;
        private ToolStripMenuItem ReturnBookToolStripMenuItem;
    }
}