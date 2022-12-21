namespace Library
{
    partial class FMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.bCatalog = new System.Windows.Forms.Button();
            this.bMembers = new System.Windows.Forms.Button();
            this.pbCatalog = new System.Windows.Forms.PictureBox();
            this.pbMembers = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbCatalog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMembers)).BeginInit();
            this.SuspendLayout();
            // 
            // bCatalog
            // 
            this.bCatalog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bCatalog.Location = new System.Drawing.Point(12, 394);
            this.bCatalog.Name = "bCatalog";
            this.bCatalog.Size = new System.Drawing.Size(280, 35);
            this.bCatalog.TabIndex = 3;
            this.bCatalog.Text = "Catalog";
            this.bCatalog.UseVisualStyleBackColor = true;
            this.bCatalog.Click += new System.EventHandler(this.bCatalog_Click);
            // 
            // bMembers
            // 
            this.bMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bMembers.Location = new System.Drawing.Point(332, 394);
            this.bMembers.Name = "bMembers";
            this.bMembers.Size = new System.Drawing.Size(280, 35);
            this.bMembers.TabIndex = 4;
            this.bMembers.Text = "Members";
            this.bMembers.UseVisualStyleBackColor = true;
            this.bMembers.Click += new System.EventHandler(this.bMembers_Click);
            // 
            // pbCatalog
            // 
            this.pbCatalog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbCatalog.Image = ((System.Drawing.Image)(resources.GetObject("pbCatalog.Image")));
            this.pbCatalog.Location = new System.Drawing.Point(12, 12);
            this.pbCatalog.Name = "pbCatalog";
            this.pbCatalog.Size = new System.Drawing.Size(280, 376);
            this.pbCatalog.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbCatalog.TabIndex = 5;
            this.pbCatalog.TabStop = false;
            // 
            // pbMembers
            // 
            this.pbMembers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMembers.Image = ((System.Drawing.Image)(resources.GetObject("pbMembers.Image")));
            this.pbMembers.Location = new System.Drawing.Point(332, 12);
            this.pbMembers.Name = "pbMembers";
            this.pbMembers.Size = new System.Drawing.Size(280, 376);
            this.pbMembers.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMembers.TabIndex = 6;
            this.pbMembers.TabStop = false;
            // 
            // FMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.pbMembers);
            this.Controls.Add(this.pbCatalog);
            this.Controls.Add(this.bMembers);
            this.Controls.Add(this.bCatalog);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FMain";
            this.Text = "Library";
            ((System.ComponentModel.ISupportInitialize)(this.pbCatalog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbMembers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Button bCatalog;
        private Button bMembers;
        private PictureBox pbCatalog;
        private PictureBox pbMembers;
    }
}