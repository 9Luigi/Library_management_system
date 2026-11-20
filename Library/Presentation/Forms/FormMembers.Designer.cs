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
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridViewForMembers).BeginInit();
			cmMember.SuspendLayout();
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
			dataGridViewForMembers.Location = new Point(14, 42);
			dataGridViewForMembers.Margin = new Padding(3, 4, 3, 4);
			dataGridViewForMembers.MultiSelect = false;
			dataGridViewForMembers.Name = "dataGridViewForMembers";
			dataGridViewForMembers.ReadOnly = true;
			dataGridViewForMembers.RowHeadersWidth = 51;
			dataGridViewForMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridViewForMembers.Size = new Size(1051, 612);
			dataGridViewForMembers.TabIndex = 2;
			dataGridViewForMembers.CellMouseClick += View_CellMouseClick;
			dataGridViewForMembers.CellMouseDoubleClick += DataGridViewForMembers_CellMouseDoubleClick;
			// 
			// cmMember
			// 
			cmMember.ImageScalingSize = new Size(20, 20);
			cmMember.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, deleteToolStripMenuItem });
			cmMember.Name = "cmMember";
			cmMember.Size = new Size(177, 52);
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
			// FormMembers
			//
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1079, 668);
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
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private MenuStrip menuStrip1;
        private ToolStripMenuItem memberToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private DataGridView dataGridViewForMembers;
        private ToolStripMenuItem addMemberToolStripMenuItem;
        private ContextMenuStrip cmMember;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
	}
}