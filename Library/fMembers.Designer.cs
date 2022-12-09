namespace Library
{
    partial class fMembers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMembers));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.memberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMemberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editOneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewForMembers = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.TbIINSearch = new System.Windows.Forms.TextBox();
            this.bIINSearch = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForMembers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.memberToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // memberToolStripMenuItem
            // 
            this.memberToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addMemberToolStripMenuItem,
            this.viewAllToolStripMenuItem,
            this.editOneToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.memberToolStripMenuItem.Name = "memberToolStripMenuItem";
            this.memberToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.memberToolStripMenuItem.Text = "Member..";
            // 
            // addMemberToolStripMenuItem
            // 
            this.addMemberToolStripMenuItem.Name = "addMemberToolStripMenuItem";
            this.addMemberToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.addMemberToolStripMenuItem.Text = "Add member";
            this.addMemberToolStripMenuItem.Click += new System.EventHandler(this.addMemberToolStripMenuItem_Click);
            // 
            // viewAllToolStripMenuItem
            // 
            this.viewAllToolStripMenuItem.Name = "viewAllToolStripMenuItem";
            this.viewAllToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.viewAllToolStripMenuItem.Text = "View all";
            this.viewAllToolStripMenuItem.Click += new System.EventHandler(this.viewAllToolStripMenuItem_Click);
            // 
            // editOneToolStripMenuItem
            // 
            this.editOneToolStripMenuItem.Name = "editOneToolStripMenuItem";
            this.editOneToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.editOneToolStripMenuItem.Text = "Edit member";
            this.editOneToolStripMenuItem.Click += new System.EventHandler(this.editOneToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.deleteToolStripMenuItem.Text = "Delete member";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // dataGridViewForMembers
            // 
            this.dataGridViewForMembers.AllowUserToAddRows = false;
            this.dataGridViewForMembers.AllowUserToDeleteRows = false;
            this.dataGridViewForMembers.AllowUserToResizeColumns = false;
            this.dataGridViewForMembers.AllowUserToResizeRows = false;
            this.dataGridViewForMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewForMembers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewForMembers.Location = new System.Drawing.Point(12, 24);
            this.dataGridViewForMembers.MultiSelect = false;
            this.dataGridViewForMembers.Name = "dataGridViewForMembers";
            this.dataGridViewForMembers.ReadOnly = true;
            this.dataGridViewForMembers.RowTemplate.Height = 25;
            this.dataGridViewForMembers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewForMembers.Size = new System.Drawing.Size(572, 354);
            this.dataGridViewForMembers.TabIndex = 2;
            this.dataGridViewForMembers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewForMembers_CellClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(590, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(210, 426);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // TbIINSearch
            // 
            this.TbIINSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbIINSearch.Location = new System.Drawing.Point(224, 384);
            this.TbIINSearch.Name = "TbIINSearch";
            this.TbIINSearch.Size = new System.Drawing.Size(148, 23);
            this.TbIINSearch.TabIndex = 4;
            this.TbIINSearch.Text = "Enter IIN";
            this.TbIINSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TbIINSearch.Leave += new System.EventHandler(this.TbIINSearch_Leave);
            this.TbIINSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TbIINSearch_MouseDown);
            // 
            // bIINSearch
            // 
            this.bIINSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bIINSearch.Location = new System.Drawing.Point(224, 413);
            this.bIINSearch.Name = "bIINSearch";
            this.bIINSearch.Size = new System.Drawing.Size(148, 23);
            this.bIINSearch.TabIndex = 5;
            this.bIINSearch.Text = "Search";
            this.bIINSearch.UseVisualStyleBackColor = true;
            this.bIINSearch.Click += new System.EventHandler(this.bIINSearch_Click);
            // 
            // fMembers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bIINSearch);
            this.Controls.Add(this.TbIINSearch);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dataGridViewForMembers);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "fMembers";
            this.Text = "Members";
            this.Load += new System.EventHandler(this.fMembers_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewForMembers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem memberToolStripMenuItem;
        private ToolStripMenuItem viewAllToolStripMenuItem;
        private ToolStripMenuItem editOneToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private BindingSource startBindingSource;
        private DataGridView dataGridViewForMembers;
        private PictureBox pictureBox1;
        private ToolStripMenuItem addMemberToolStripMenuItem;
        private TextBox TbIINSearch;
        private Button bIINSearch;
    }
}