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
			DataGridViewForLendBook.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			DataGridViewForLendBook.Location = new Point(13, 20);
			DataGridViewForLendBook.Name = "DataGridViewForLendBook";
			DataGridViewForLendBook.RowHeadersWidth = 51;
			DataGridViewForLendBook.Size = new Size(776, 368);
			DataGridViewForLendBook.TabIndex = 0;
			DataGridViewForLendBook.CellMouseClick += DataGridViewForLendBook_CellMouseClick;
			// 
			// CMSLendBook
			// 
			CMSLendBook.ImageScalingSize = new Size(20, 20);
			CMSLendBook.Items.AddRange(new ToolStripItem[] { lendABookToolStripMenuItem, returnBookToolStripMenuItem });
			CMSLendBook.Name = "cmsLendBook";
			CMSLendBook.Size = new Size(152, 48);
			// 
			// lendABookToolStripMenuItem
			// 
			lendABookToolStripMenuItem.Name = "lendABookToolStripMenuItem";
			lendABookToolStripMenuItem.Size = new Size(151, 22);
			lendABookToolStripMenuItem.Text = "Borrow a book";
			lendABookToolStripMenuItem.Click += BorrowABookToolStripMenuItem_Click;
			// 
			// returnBookToolStripMenuItem
			// 
			returnBookToolStripMenuItem.Name = "returnBookToolStripMenuItem";
			returnBookToolStripMenuItem.Size = new Size(151, 22);
			returnBookToolStripMenuItem.Text = "Return book";
			returnBookToolStripMenuItem.Click += ReturnBookToolStripMenuItem_Click;
			// 
			// PBBorrowOrReturn
			// 
			PBBorrowOrReturn.Location = new Point(12, 386);
			PBBorrowOrReturn.Name = "PBBorrowOrReturn";
			PBBorrowOrReturn.Size = new Size(776, 23);
			PBBorrowOrReturn.TabIndex = 1;
			// 
			// FormBorrowOrRecieveBook
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(PBBorrowOrReturn);
			Controls.Add(DataGridViewForLendBook);
			Name = "FormBorrowOrRecieveBook";
			FormClosed += FormBorrowOrRecieveBook_FormClosed;
			Load += FormBorrowOrRecieveBook_Load;
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