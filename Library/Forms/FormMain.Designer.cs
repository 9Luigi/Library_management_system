namespace Library
{
    partial class FormMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			bCatalog = new Button();
			bMembers = new Button();
			pbCatalog = new PictureBox();
			pbMembers = new PictureBox();
			((System.ComponentModel.ISupportInitialize)pbCatalog).BeginInit();
			((System.ComponentModel.ISupportInitialize)pbMembers).BeginInit();
			SuspendLayout();
			// 
			// bCatalog
			// 
			bCatalog.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			bCatalog.Location = new Point(12, 394);
			bCatalog.Name = "bCatalog";
			bCatalog.Size = new Size(280, 35);
			bCatalog.TabIndex = 3;
			bCatalog.Text = "Catalog";
			bCatalog.UseVisualStyleBackColor = true;
			bCatalog.Click += bCatalog_Click;
			// 
			// bMembers
			// 
			bMembers.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			bMembers.Location = new Point(332, 394);
			bMembers.Name = "bMembers";
			bMembers.Size = new Size(280, 35);
			bMembers.TabIndex = 4;
			bMembers.Text = "Members";
			bMembers.UseVisualStyleBackColor = true;
			bMembers.Click += bMembers_Click;
			// 
			// pbCatalog
			// 
			pbCatalog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			pbCatalog.Image = (Image)resources.GetObject("pbCatalog.Image");
			pbCatalog.Location = new Point(12, 12);
			pbCatalog.Name = "pbCatalog";
			pbCatalog.Size = new Size(280, 376);
			pbCatalog.SizeMode = PictureBoxSizeMode.StretchImage;
			pbCatalog.TabIndex = 5;
			pbCatalog.TabStop = false;
			// 
			// pbMembers
			// 
			pbMembers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
			pbMembers.Image = (Image)resources.GetObject("pbMembers.Image");
			pbMembers.Location = new Point(332, 12);
			pbMembers.Name = "pbMembers";
			pbMembers.Size = new Size(280, 376);
			pbMembers.SizeMode = PictureBoxSizeMode.StretchImage;
			pbMembers.TabIndex = 6;
			pbMembers.TabStop = false;
			// 
			// FMain
			// 
			AutoScaleDimensions = new SizeF(9F, 19F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(624, 441);
			Controls.Add(pbMembers);
			Controls.Add(pbCatalog);
			Controls.Add(bMembers);
			Controls.Add(bCatalog);
			Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
			Margin = new Padding(4);
			Name = "FMain";
			Text = "Library";
			Load += FMain_Load;
			((System.ComponentModel.ISupportInitialize)pbCatalog).EndInit();
			((System.ComponentModel.ISupportInitialize)pbMembers).EndInit();
			ResumeLayout(false);
		}

		#endregion

		private Button bCatalog;
        private Button bMembers;
        private PictureBox pbCatalog;
        private PictureBox pbMembers;
    }
}