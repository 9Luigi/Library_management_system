namespace Library
{
    partial class FormCatalog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCatalog));
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
			pbMembers = new ProgressBar();
			TbIINSearch = new TextBox();
			pictureBox1 = new PictureBox();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewForMembers).BeginInit();
			cmMember.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			// 
			// menuStrip1
			// 
			menuStrip1.ImageScalingSize = new Size(20, 20);
			menuStrip1.Items.AddRange(new ToolStripItem[] { memberToolStripMenuItem, exitToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Padding = new Padding(7, 3, 0, 3);
			menuStrip1.Size = new Size(914, 30);
			menuStrip1.TabIndex = 7;
			menuStrip1.Text = "menuStrip1";
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
			// 
			// exitToolStripMenuItem
			// 
			exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			exitToolStripMenuItem.Size = new Size(47, 24);
			exitToolStripMenuItem.Text = "Exit";
			// 
			// dataGridViewForMembers
			// 
			dataGridViewForMembers.AllowUserToAddRows = false;
			dataGridViewForMembers.AllowUserToDeleteRows = false;
			dataGridViewForMembers.AllowUserToResizeColumns = false;
			dataGridViewForMembers.AllowUserToResizeRows = false;
			dataGridViewForMembers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridViewForMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridViewForMembers.Location = new Point(14, 69);
			dataGridViewForMembers.Margin = new Padding(3, 4, 3, 4);
			dataGridViewForMembers.MultiSelect = false;
			dataGridViewForMembers.Name = "dataGridViewForMembers";
			dataGridViewForMembers.ReadOnly = true;
			dataGridViewForMembers.RowHeadersWidth = 51;
			dataGridViewForMembers.RowTemplate.Height = 25;
			dataGridViewForMembers.SelectionMode = DataGridViewSelectionMode.CellSelect;
			dataGridViewForMembers.Size = new Size(654, 421);
			dataGridViewForMembers.TabIndex = 8;
			// 
			// cmMember
			// 
			cmMember.ImageScalingSize = new Size(20, 20);
			cmMember.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem, leToolStripMenuItem, seeLendedBooksForThisMemberToolStripMenuItem });
			cmMember.Name = "cmMember";
			cmMember.Size = new Size(275, 100);
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(274, 24);
			editToolStripMenuItem.Text = "Edit";
			// 
			// deleteToolStripMenuItem
			// 
			deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			deleteToolStripMenuItem.Size = new Size(274, 24);
			deleteToolStripMenuItem.Text = "Delete";
			// 
			// leToolStripMenuItem
			// 
			leToolStripMenuItem.Name = "leToolStripMenuItem";
			leToolStripMenuItem.Size = new Size(274, 24);
			leToolStripMenuItem.Text = "Borrow a book";
			// 
			// seeLendedBooksForThisMemberToolStripMenuItem
			// 
			seeLendedBooksForThisMemberToolStripMenuItem.Name = "seeLendedBooksForThisMemberToolStripMenuItem";
			seeLendedBooksForThisMemberToolStripMenuItem.Size = new Size(274, 24);
			seeLendedBooksForThisMemberToolStripMenuItem.Text = "Borrowed books/ return book";
			// 
			// pbMembers
			// 
			pbMembers.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			pbMembers.Location = new Point(14, 498);
			pbMembers.Margin = new Padding(3, 4, 3, 4);
			pbMembers.Name = "pbMembers";
			pbMembers.Size = new Size(887, 31);
			pbMembers.Step = 1;
			pbMembers.TabIndex = 11;
			// 
			// TbIINSearch
			// 
			TbIINSearch.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			TbIINSearch.Location = new Point(373, 537);
			TbIINSearch.Margin = new Padding(3, 4, 3, 4);
			TbIINSearch.Name = "TbIINSearch";
			TbIINSearch.Size = new Size(169, 27);
			TbIINSearch.TabIndex = 10;
			TbIINSearch.Text = "Enter IIN";
			TbIINSearch.TextAlign = HorizontalAlignment.Center;
			// 
			// pictureBox1
			// 
			pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
			pictureBox1.Location = new Point(674, 69);
			pictureBox1.Margin = new Padding(3, 4, 3, 4);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(226, 421);
			pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
			pictureBox1.TabIndex = 9;
			pictureBox1.TabStop = false;
			// 
			// FCatalog
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(914, 600);
			Controls.Add(menuStrip1);
			Controls.Add(dataGridViewForMembers);
			Controls.Add(pbMembers);
			Controls.Add(TbIINSearch);
			Controls.Add(pictureBox1);
			Margin = new Padding(3, 4, 3, 4);
			Name = "FCatalog";
			Text = "Catalog";
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
		private ToolStripMenuItem addMemberToolStripMenuItem;
		private ToolStripMenuItem exitToolStripMenuItem;
		private DataGridView dataGridViewForMembers;
		private ContextMenuStrip cmMember;
		private ToolStripMenuItem editToolStripMenuItem;
		private ToolStripMenuItem deleteToolStripMenuItem;
		private ToolStripMenuItem leToolStripMenuItem;
		private ToolStripMenuItem seeLendedBooksForThisMemberToolStripMenuItem;
		private ProgressBar pbMembers;
		private TextBox TbIINSearch;
		private PictureBox pictureBox1;
	}
}