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
			TBSurname.Location = new Point(14, 96);
			TBSurname.Margin = new Padding(3, 4, 3, 4);
			TBSurname.Name = "TBSurname";
			TBSurname.Size = new Size(114, 27);
			TBSurname.TabIndex = 1;
			TBSurname.Click += TextBoxBase_OnClick;
			TBSurname.Enter += TextBoxBase_OnFocusEnter;
			// 
			// TBPatronymic
			// 
			TBPatronymic.Location = new Point(14, 147);
			TBPatronymic.Margin = new Padding(3, 4, 3, 4);
			TBPatronymic.Name = "TBPatronymic";
			TBPatronymic.Size = new Size(114, 27);
			TBPatronymic.TabIndex = 2;
			TBPatronymic.Click += TextBoxBase_OnClick;
			TBPatronymic.Enter += TextBoxBase_OnFocusEnter;
			// 
			// TBAge
			// 
			TBAge.Location = new Point(14, 248);
			TBAge.Margin = new Padding(3, 4, 3, 4);
			TBAge.Name = "TBAge";
			TBAge.Size = new Size(114, 27);
			TBAge.TabIndex = 4;
			TBAge.Click += TextBoxBase_OnClick;
			TBAge.Enter += TextBoxBase_OnFocusEnter;
			// 
			// pbPhoto
			// 
			pbPhoto.Location = new Point(263, 16);
			pbPhoto.Margin = new Padding(3, 4, 3, 4);
			pbPhoto.Name = "pbPhoto";
			pbPhoto.Size = new Size(333, 437);
			pbPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
			pbPhoto.TabIndex = 9;
			pbPhoto.TabStop = false;
			// 
			// BSelectPhoto
			// 
			BSelectPhoto.Location = new Point(370, 480);
			BSelectPhoto.Margin = new Padding(3, 4, 3, 4);
			BSelectPhoto.Name = "BSelectPhoto";
			BSelectPhoto.Size = new Size(115, 31);
			BSelectPhoto.TabIndex = 8;
			BSelectPhoto.Text = "Select photo*";
			BSelectPhoto.UseVisualStyleBackColor = true;
			BSelectPhoto.Click += BSelectPhoto_Click;
			// 
			// BAddMember
			// 
			BAddMember.Location = new Point(107, 540);
			BAddMember.Margin = new Padding(3, 4, 3, 4);
			BAddMember.Name = "BAddMember";
			BAddMember.Size = new Size(136, 61);
			BAddMember.TabIndex = 9;
			BAddMember.Text = "Add member";
			BAddMember.UseVisualStyleBackColor = true;
			BAddMember.Click += BAddMember_Click;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(135, 49);
			label1.Name = "label1";
			label1.Size = new Size(55, 20);
			label1.TabIndex = 12;
			label1.Text = "Name*";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(135, 100);
			label2.Name = "label2";
			label2.Size = new Size(73, 20);
			label2.TabIndex = 13;
			label2.Text = "Surname*";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(135, 151);
			label3.Name = "label3";
			label3.Size = new Size(82, 20);
			label3.TabIndex = 14;
			label3.Text = "Patronymic";
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(135, 201);
			label4.Name = "label4";
			label4.Size = new Size(34, 20);
			label4.TabIndex = 15;
			label4.Text = "IIN*";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(135, 252);
			label5.Name = "label5";
			label5.Size = new Size(36, 20);
			label5.TabIndex = 16;
			label5.Text = "Age";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new Point(135, 303);
			label6.Name = "label6";
			label6.Size = new Size(70, 20);
			label6.TabIndex = 17;
			label6.Text = "Birthday*";
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new Point(135, 353);
			label7.Name = "label7";
			label7.Size = new Size(53, 20);
			label7.TabIndex = 18;
			label7.Text = "Adress";
			// 
			// label8
			// 
			label8.AutoSize = true;
			label8.Location = new Point(135, 409);
			label8.Name = "label8";
			label8.Size = new Size(105, 20);
			label8.TabIndex = 19;
			label8.Text = "Phone number";
			// 
			// MTBPhoneNumber
			// 
			MTBPhoneNumber.Location = new Point(14, 405);
			MTBPhoneNumber.Margin = new Padding(3, 4, 3, 4);
			MTBPhoneNumber.Mask = "+7(999) 000-0000";
			MTBPhoneNumber.Name = "MTBPhoneNumber";
			MTBPhoneNumber.Size = new Size(114, 27);
			MTBPhoneNumber.TabIndex = 7;
			MTBPhoneNumber.Click += TextBoxBase_OnClick;
			MTBPhoneNumber.Enter += TextBoxBase_OnFocusEnter;
			// 
			// MTBBirthday
			// 
			MTBBirthday.Culture = new System.Globalization.CultureInfo("");
			MTBBirthday.Location = new Point(14, 299);
			MTBBirthday.Margin = new Padding(3, 4, 3, 4);
			MTBBirthday.Mask = "00.00.0000";
			MTBBirthday.Name = "MTBBirthday";
			MTBBirthday.Size = new Size(114, 27);
			MTBBirthday.TabIndex = 5;
			MTBBirthday.ValidatingType = typeof(DateTime);
			MTBBirthday.Click += TextBoxBase_OnClick;
			MTBBirthday.Enter += TextBoxBase_OnFocusEnter;
			// 
			// MTBIIN
			// 
			MTBIIN.Location = new Point(14, 197);
			MTBIIN.Margin = new Padding(3, 4, 3, 4);
			MTBIIN.Mask = "000000000000";
			MTBIIN.Name = "MTBIIN";
			MTBIIN.Size = new Size(114, 27);
			MTBIIN.TabIndex = 3;
			MTBIIN.ValidatingType = typeof(int);
			MTBIIN.Click += TextBoxBase_OnClick;
			MTBIIN.Enter += TextBoxBase_OnFocusEnter;
			// 
			// TBName
			// 
			TBName.Location = new Point(14, 45);
			TBName.Margin = new Padding(3, 4, 3, 4);
			TBName.Name = "TBName";
			TBName.Size = new Size(114, 27);
			TBName.TabIndex = 0;
			TBName.Click += TextBoxBase_OnClick;
			TBName.Enter += TextBoxBase_OnFocusEnter;
			// 
			// MTBAdress
			// 
			MTBAdress.Location = new Point(14, 349);
			MTBAdress.Margin = new Padding(3, 4, 3, 4);
			MTBAdress.Name = "MTBAdress";
			MTBAdress.Size = new Size(114, 27);
			MTBAdress.TabIndex = 6;
			// 
			// BUpdateMember
			// 
			BUpdateMember.Location = new Point(336, 540);
			BUpdateMember.Margin = new Padding(3, 4, 3, 4);
			BUpdateMember.Name = "BUpdateMember";
			BUpdateMember.Size = new Size(136, 61);
			BUpdateMember.TabIndex = 10;
			BUpdateMember.Text = "Update member";
			BUpdateMember.UseVisualStyleBackColor = true;
			BUpdateMember.Click += BUpdateMember_Click;
			// 
			// AdressLabelHint
			// 
			AdressLabelHint.AutoSize = true;
			AdressLabelHint.Location = new Point(2, 380);
			AdressLabelHint.Name = "AdressLabelHint";
			AdressLabelHint.Size = new Size(267, 20);
			AdressLabelHint.TabIndex = 20;
			AdressLabelHint.Text = "Adress must be like Amangeldy 192-13";
			// 
			// FaddEdit_prop
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(609, 645);
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
			Margin = new Padding(3, 4, 3, 4);
			Name = "FaddEdit_prop";
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