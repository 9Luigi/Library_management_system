namespace Library
{
    partial class FormBorrowOrReturnBook
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
			DataGridViewForLendBook = new DataGridView();
			CMSLendBook = new ContextMenuStrip(components);
			lendABookToolStripMenuItem = new ToolStripMenuItem();
			returnBookToolStripMenuItem = new ToolStripMenuItem();
			PBBorrowOrReturn = new ProgressBar();
			unlendABookToolStripMenuItem = new ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)DataGridViewForLendBook).BeginInit();
			CMSLendBook.SuspendLayout();
			SuspendLayout();
			// 
			// DataGridViewForLendBook
			// 
			DataGridViewForLendBook.AllowUserToAddRows = false;
			DataGridViewForLendBook.AllowUserToDeleteRows = false;
			DataGridViewForLendBook.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			DataGridViewForLendBook.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			DataGridViewForLendBook.Location = new Point(16, 16);
			DataGridViewForLendBook.Margin = new Padding(3, 4, 3, 4);
			DataGridViewForLendBook.MultiSelect = false;
			DataGridViewForLendBook.Name = "DataGridViewForLendBook";
			DataGridViewForLendBook.ReadOnly = true;
			DataGridViewForLendBook.RowHeadersWidth = 51;
			DataGridViewForLendBook.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			DataGridViewForLendBook.Size = new Size(882, 528);
			DataGridViewForLendBook.TabIndex = 0;
			DataGridViewForLendBook.CellMouseClick += DataGridViewForLendBook_CellMouseClick;
			// 
			// CMSLendBook
			// 
			CMSLendBook.ImageScalingSize = new Size(20, 20);
			CMSLendBook.Items.AddRange(new ToolStripItem[] { lendABookToolStripMenuItem, returnBookToolStripMenuItem });
			CMSLendBook.Name = "cmsLendBook";
			CMSLendBook.Size = new Size(177, 52);
			// 
			// lendABookToolStripMenuItem
			// 
			lendABookToolStripMenuItem.Name = "lendABookToolStripMenuItem";
			lendABookToolStripMenuItem.Size = new Size(176, 24);
			lendABookToolStripMenuItem.Text = "Borrow a book";
			lendABookToolStripMenuItem.Click += BorrowABookToolStripMenuItem_Click;
			// 
			// returnBookToolStripMenuItem
			// 
			returnBookToolStripMenuItem.Name = "returnBookToolStripMenuItem";
			returnBookToolStripMenuItem.Size = new Size(176, 24);
			returnBookToolStripMenuItem.Text = "Return book";
			returnBookToolStripMenuItem.Click += ReturnBookToolStripMenuItem_Click;
			//
			// PBBorrowOrReturn
			//
			PBBorrowOrReturn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			PBBorrowOrReturn.Location = new Point(16, 568);
			PBBorrowOrReturn.Margin = new Padding(3, 4, 3, 4);
			PBBorrowOrReturn.Name = "PBBorrowOrReturn";
			PBBorrowOrReturn.Size = new Size(882, 6);
			PBBorrowOrReturn.TabIndex = 1;
			//
			// unlendABookToolStripMenuItem
			// 
			unlendABookToolStripMenuItem.Name = "unlendABookToolStripMenuItem";
			unlendABookToolStripMenuItem.Size = new Size(32, 19);
			// 
			// FormBorrowOrReturnBook
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(914, 600);
			Controls.Add(PBBorrowOrReturn);
			Controls.Add(DataGridViewForLendBook);
			Margin = new Padding(3, 4, 3, 4);
			Name = "FormBorrowOrReturnBook";
			FormClosed += FormBorrowOrRecieveBook_FormClosed;
			Load += FormBorrowOrReturnBook_Load;
			((System.ComponentModel.ISupportInitialize)DataGridViewForLendBook).EndInit();
			CMSLendBook.ResumeLayout(false);
			ResumeLayout(false);
		}

		#endregion

		private DataGridView DataGridViewForLendBook;
        private ContextMenuStrip CMSLendBook;
        private ToolStripMenuItem lendABookToolStripMenuItem;
        private ProgressBar PBBorrowOrReturn;
		private ToolStripMenuItem unlendABookToolStripMenuItem;
		private ToolStripMenuItem returnBookToolStripMenuItem;
	}
}