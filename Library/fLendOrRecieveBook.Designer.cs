namespace Library
{
    partial class fLendOrRecieveBook
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
            this.dataGridViewForLendBook = new System.Windows.Forms.DataGridView();
            this.cmsLendBook = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lendABookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unlendABookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pbBorrowOrReturn = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForLendBook)).BeginInit();
            this.cmsLendBook.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewForLendBook
            // 
            this.dataGridViewForLendBook.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewForLendBook.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewForLendBook.Name = "dataGridViewForLendBook";
            this.dataGridViewForLendBook.RowTemplate.Height = 25;
            this.dataGridViewForLendBook.Size = new System.Drawing.Size(776, 368);
            this.dataGridViewForLendBook.TabIndex = 0;
            this.dataGridViewForLendBook.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewForLendBook_CellMouseClick);
            // 
            // cmsLendBook
            // 
            this.cmsLendBook.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lendABookToolStripMenuItem,
            this.unlendABookToolStripMenuItem});
            this.cmsLendBook.Name = "cmsLendBook";
            this.cmsLendBook.Size = new System.Drawing.Size(152, 48);
            // 
            // lendABookToolStripMenuItem
            // 
            this.lendABookToolStripMenuItem.Name = "lendABookToolStripMenuItem";
            this.lendABookToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.lendABookToolStripMenuItem.Text = "Borrow a book";
            this.lendABookToolStripMenuItem.Click += new System.EventHandler(this.lendABookToolStripMenuItem_Click);
            // 
            // unlendABookToolStripMenuItem
            // 
            this.unlendABookToolStripMenuItem.Name = "unlendABookToolStripMenuItem";
            this.unlendABookToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.unlendABookToolStripMenuItem.Text = "Return a book";
            this.unlendABookToolStripMenuItem.Click += new System.EventHandler(this.unlendABookToolStripMenuItem_Click);
            // 
            // pbBorrowOrReturn
            // 
            this.pbBorrowOrReturn.Location = new System.Drawing.Point(12, 386);
            this.pbBorrowOrReturn.Name = "pbBorrowOrReturn";
            this.pbBorrowOrReturn.Size = new System.Drawing.Size(776, 23);
            this.pbBorrowOrReturn.TabIndex = 1;
            // 
            // fLendOrRecieveBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbBorrowOrReturn);
            this.Controls.Add(this.dataGridViewForLendBook);
            this.Name = "fLendOrRecieveBook";
            this.Load += new System.EventHandler(this.fLendOrRecieveBook_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForLendBook)).EndInit();
            this.cmsLendBook.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridView dataGridViewForLendBook;
        private ContextMenuStrip cmsLendBook;
        private ToolStripMenuItem lendABookToolStripMenuItem;
        private ToolStripMenuItem unlendABookToolStripMenuItem;
        private ProgressBar pbBorrowOrReturn;
    }
}