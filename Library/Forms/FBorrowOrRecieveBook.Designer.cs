namespace Library
{
    partial class FBorrowOrRecieveBook
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
            this.components = new System.ComponentModel.Container();
            this.DataGridViewForLendBook = new System.Windows.Forms.DataGridView();
            this.CMSLendBook = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lendABookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unlendABookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PBBorrowOrReturn = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewForLendBook)).BeginInit();
            this.CMSLendBook.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataGridViewForLendBook
            // 
            this.DataGridViewForLendBook.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataGridViewForLendBook.Location = new System.Drawing.Point(12, 12);
            this.DataGridViewForLendBook.Name = "DataGridViewForLendBook";
            this.DataGridViewForLendBook.RowTemplate.Height = 25;
            this.DataGridViewForLendBook.Size = new System.Drawing.Size(776, 368);
            this.DataGridViewForLendBook.TabIndex = 0;
            this.DataGridViewForLendBook.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewForLendBook_CellMouseClick);
            // 
            // CMSLendBook
            // 
            this.CMSLendBook.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lendABookToolStripMenuItem,
            this.unlendABookToolStripMenuItem});
            this.CMSLendBook.Name = "cmsLendBook";
            this.CMSLendBook.Size = new System.Drawing.Size(152, 48);
            // 
            // lendABookToolStripMenuItem
            // 
            this.lendABookToolStripMenuItem.Name = "lendABookToolStripMenuItem";
            this.lendABookToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.lendABookToolStripMenuItem.Text = "Borrow a book";
            this.lendABookToolStripMenuItem.Click += new System.EventHandler(this.LendABookToolStripMenuItem_Click);
            // 
            // unlendABookToolStripMenuItem
            // 
            this.unlendABookToolStripMenuItem.Name = "unlendABookToolStripMenuItem";
            this.unlendABookToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.unlendABookToolStripMenuItem.Text = "Return a book";
            this.unlendABookToolStripMenuItem.Click += new System.EventHandler(this.UnlendABookToolStripMenuItem_Click);
            // 
            // PBBorrowOrReturn
            // 
            this.PBBorrowOrReturn.Location = new System.Drawing.Point(12, 386);
            this.PBBorrowOrReturn.Name = "PBBorrowOrReturn";
            this.PBBorrowOrReturn.Size = new System.Drawing.Size(776, 23);
            this.PBBorrowOrReturn.TabIndex = 1;
            // 
            // FlendOrRecieveBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PBBorrowOrReturn);
            this.Controls.Add(this.DataGridViewForLendBook);
            this.Name = "FlendOrRecieveBook";
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewForLendBook)).EndInit();
            this.CMSLendBook.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView DataGridViewForLendBook;
        private ContextMenuStrip CMSLendBook;
        private ToolStripMenuItem lendABookToolStripMenuItem;
        private ToolStripMenuItem unlendABookToolStripMenuItem;
        private ProgressBar PBBorrowOrReturn;
    }
}