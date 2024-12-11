namespace Library
{
    partial class FormBorrowOrRecieveBook
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
			DataGridViewForLendBook.Location = new Point(15, 26);
			DataGridViewForLendBook.Margin = new Padding(3, 4, 3, 4);
			DataGridViewForLendBook.Name = "DataGridViewForLendBook";
			DataGridViewForLendBook.RowHeadersWidth = 51;
			DataGridViewForLendBook.RowTemplate.Height = 25;
			DataGridViewForLendBook.Size = new Size(887, 491);
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
			lendABookToolStripMenuItem.Click += LendABookToolStripMenuItem_Click;
			// 
			// returnBookToolStripMenuItem
			// 
			returnBookToolStripMenuItem.Name = "returnBookToolStripMenuItem";
			returnBookToolStripMenuItem.Size = new Size(176, 24);
			returnBookToolStripMenuItem.Text = "Return book";
			// 
			// PBBorrowOrReturn
			// 
			PBBorrowOrReturn.Location = new Point(14, 515);
			PBBorrowOrReturn.Margin = new Padding(3, 4, 3, 4);
			PBBorrowOrReturn.Name = "PBBorrowOrReturn";
			PBBorrowOrReturn.Size = new Size(887, 31);
			PBBorrowOrReturn.TabIndex = 1;
			// 
			// unlendABookToolStripMenuItem
			// 
			unlendABookToolStripMenuItem.Name = "unlendABookToolStripMenuItem";
			unlendABookToolStripMenuItem.Size = new Size(180, 22);
			unlendABookToolStripMenuItem.Text = "Return a book";
			unlendABookToolStripMenuItem.Click += UnlendABookToolStripMenuItem_Click;
			// 
			// FormBorrowOrRecieveBook
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(914, 600);
			Controls.Add(PBBorrowOrReturn);
			Controls.Add(DataGridViewForLendBook);
			Margin = new Padding(3, 4, 3, 4);
			Name = "FormBorrowOrRecieveBook";
			FormClosed += FormBorrowOrRecieveBook_FormClosed;
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