namespace Library
{
    partial class FaddEdit_prop
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
			TBSurname = new TextBox();
			TBPatronymic = new TextBox();
			TBAge = new TextBox();
			pbPhoto = new PictureBox();
			BSelectPhoto = new Button();
			BAddMember = new Button();
			label1 = new Label();
			label2 = new Label();
			label3 = new Label();
			label4 = new Label();
			label5 = new Label();
			label6 = new Label();
			label7 = new Label();
			label8 = new Label();
			MTBPhoneNumber = new MaskedTextBox();
			MTBBirthday = new MaskedTextBox();
			MTBIIN = new MaskedTextBox();
			TBName = new TextBox();
			MTBAdress = new MaskedTextBox();
			BUpdateMember = new Button();
			AdressLabelHint = new Label();
			((System.ComponentModel.ISupportInitialize)pbPhoto).BeginInit();
			SuspendLayout();
			// 
			// TBSurname
			// 
			TBSurname.Location = new Point(12, 50);
			TBSurname.Name = "TBSurname";
			TBSurname.Size = new Size(100, 23);
			TBSurname.TabIndex = 1;
			// 
			// TBPatronymic
			// 
			TBPatronymic.Location = new Point(12, 88);
			TBPatronymic.Name = "TBPatronymic";
			TBPatronymic.Size = new Size(100, 23);
			TBPatronymic.TabIndex = 2;
			// 
			// TBAge
			// 
			TBAge.Location = new Point(12, 164);
			TBAge.Name = "TBAge";
			TBAge.Size = new Size(100, 23);
			TBAge.TabIndex = 4;
			// 
			// pbPhoto
			// 
			pbPhoto.Location = new Point(230, 12);
			pbPhoto.Name = "pbPhoto";
			pbPhoto.Size = new Size(240, 360);
			pbPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
			pbPhoto.TabIndex = 9;
			pbPhoto.TabStop = false;
			// 
			// BSelectPhoto
			// 
			BSelectPhoto.Location = new Point(295, 378);
			BSelectPhoto.Name = "BSelectPhoto";
			BSelectPhoto.Size = new Size(101, 23);
			BSelectPhoto.TabIndex = 8;
			BSelectPhoto.Text = "Select photo*";
			BSelectPhoto.UseVisualStyleBackColor = true;
			BSelectPhoto.Click += BSelectPhoto_Click;
			// 
			// BAddMember
			// 
			BAddMember.Location = new Point(12, 355);
			BAddMember.Name = "BAddMember";
			BAddMember.Size = new Size(119, 46);
			BAddMember.TabIndex = 9;
			BAddMember.Text = "Add member";
			BAddMember.UseVisualStyleBackColor = true;
			BAddMember.Click += BAddMember_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(118, 15);
			label1.Name = "label1";
			label1.Size = new Size(44, 15);
			label1.TabIndex = 12;
			label1.Text = "Name*";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(118, 53);
			label2.Name = "label2";
			label2.Size = new Size(59, 15);
			label2.TabIndex = 13;
			label2.Text = "Surname*";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(118, 91);
			label3.Name = "label3";
			label3.Size = new Size(68, 15);
			label3.TabIndex = 14;
			label3.Text = "Patronymic";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(118, 129);
			label4.Name = "label4";
			label4.Size = new Size(27, 15);
			label4.TabIndex = 15;
			label4.Text = "IIN*";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(118, 167);
			label5.Name = "label5";
			label5.Size = new Size(28, 15);
			label5.TabIndex = 16;
			label5.Text = "Age";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new Point(118, 205);
			label6.Name = "label6";
			label6.Size = new Size(56, 15);
			label6.TabIndex = 17;
			label6.Text = "Birthday*";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new Point(118, 243);
			label7.Name = "label7";
			label7.Size = new Size(42, 15);
			label7.TabIndex = 18;
			label7.Text = "Adress";
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new Point(118, 285);
			label8.Name = "label8";
			label8.Size = new Size(86, 15);
			label8.TabIndex = 19;
			label8.Text = "Phone number";
			// 
			// MTBPhoneNumber
			// 
			MTBPhoneNumber.Location = new Point(12, 282);
			MTBPhoneNumber.Mask = "+7(999) 000-0000";
			MTBPhoneNumber.Name = "MTBPhoneNumber";
			MTBPhoneNumber.Size = new Size(100, 23);
			MTBPhoneNumber.TabIndex = 7;
			// 
			// MTBBirthday
			// 
			MTBBirthday.Culture = new System.Globalization.CultureInfo("");
			MTBBirthday.Location = new Point(12, 202);
			MTBBirthday.Mask = "00.00.0000";
			MTBBirthday.Name = "MTBBirthday";
			MTBBirthday.Size = new Size(100, 23);
			MTBBirthday.TabIndex = 5;
			MTBBirthday.ValidatingType = typeof(DateTime);
			// 
			// MTBIIN
			// 
			MTBIIN.Location = new Point(12, 126);
			MTBIIN.Mask = "000000000000";
			MTBIIN.Name = "MTBIIN";
			MTBIIN.Size = new Size(100, 23);
			MTBIIN.TabIndex = 3;
			MTBIIN.ValidatingType = typeof(int);
			// 
			// TBName
			// 
			TBName.Location = new Point(12, 12);
			TBName.Name = "TBName";
			TBName.Size = new Size(100, 23);
			TBName.TabIndex = 0;
			// 
			// MTBAdress
			// 
			MTBAdress.Location = new Point(12, 240);
			MTBAdress.Name = "MTBAdress";
			MTBAdress.Size = new Size(100, 23);
			MTBAdress.TabIndex = 6;
			// 
			// BUpdateMember
			// 
			BUpdateMember.Location = new Point(12, 355);
			BUpdateMember.Name = "BUpdateMember";
			BUpdateMember.Size = new Size(119, 46);
			BUpdateMember.TabIndex = 10;
			BUpdateMember.Text = "Update member";
			BUpdateMember.UseVisualStyleBackColor = true;
			BUpdateMember.Click += BUpdateMember_Click;
			// 
			// AdressLabelHint
			// 
			AdressLabelHint.AutoSize = true;
			AdressLabelHint.Location = new Point(12, 264);
			AdressLabelHint.Name = "AdressLabelHint";
			AdressLabelHint.Size = new Size(211, 15);
			AdressLabelHint.TabIndex = 20;
			AdressLabelHint.Text = "Adress must be like Amangeldy 192-13";
			// 
			// FaddEdit_prop
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(495, 519);
			Controls.Add(AdressLabelHint);
			Controls.Add(BUpdateMember);
			Controls.Add(MTBAdress);
			Controls.Add(MTBIIN);
			Controls.Add(MTBBirthday);
			Controls.Add(MTBPhoneNumber);
			Controls.Add(label8);
			Controls.Add(label7);
			Controls.Add(label6);
			Controls.Add(label5);
			Controls.Add(label4);
			Controls.Add(label3);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(BAddMember);
			Controls.Add(BSelectPhoto);
			Controls.Add(pbPhoto);
			Controls.Add(TBAge);
			Controls.Add(TBPatronymic);
			Controls.Add(TBSurname);
			Controls.Add(TBName);
			Name = "FaddEdit_prop";
			FormClosing += FaddEdit_prop_FormClosing;
			((System.ComponentModel.ISupportInitialize)pbPhoto).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private TextBox TBSurname;
        private TextBox TBPatronymic;
        private TextBox TBAge;
        private PictureBox pbPhoto;
        private Button BSelectPhoto;
        private Button BAddMember;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private MaskedTextBox MTBPhoneNumber;
        private MaskedTextBox MTBBirthday;
        private MaskedTextBox MTBIIN;
        private TextBox TBName;
        private MaskedTextBox MTBAdress;
        private Button BUpdateMember;
		private Label AdressLabelHint;
	}
}