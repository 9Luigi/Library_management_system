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
			lblPersonal = new Label();
			lblContacts = new Label();
			label2 = new Label();
			TBSurname = new TextBox();
			label1 = new Label();
			TBName = new TextBox();
			label3 = new Label();
			TBPatronymic = new TextBox();
			label6 = new Label();
			MTBBirthday = new MaskedTextBox();
			label5 = new Label();
			TBAge = new TextBox();
			label4 = new Label();
			MTBIIN = new MaskedTextBox();
			label8 = new Label();
			MTBPhoneNumber = new MaskedTextBox();
			label7 = new Label();
			MTBAdress = new MaskedTextBox();
			BCancel = new Button();
			BAddMember = new Button();
			BUpdateMember = new Button();
			SuspendLayout();
			//
			// lblPersonal
			//
			lblPersonal.AutoSize = true;
			lblPersonal.Location = new Point(24, 16);
			lblPersonal.Name = "lblPersonal";
			lblPersonal.Size = new Size(72, 20);
			lblPersonal.TabIndex = 20;
			lblPersonal.Text = "PERSONAL";
			//
			// lblContacts
			//
			lblContacts.AutoSize = true;
			lblContacts.Location = new Point(320, 16);
			lblContacts.Name = "lblContacts";
			lblContacts.Size = new Size(110, 20);
			lblContacts.TabIndex = 21;
			lblContacts.Text = "ID && CONTACTS";
			//
			// label2
			//
			label2.AutoSize = true;
			label2.Location = new Point(24, 48);
			label2.Name = "label2";
			label2.Size = new Size(73, 20);
			label2.TabIndex = 13;
			label2.Text = "Surname*";
			//
			// TBSurname
			//
			TBSurname.Location = new Point(24, 70);
			TBSurname.Margin = new Padding(3, 4, 3, 4);
			TBSurname.Name = "TBSurname";
			TBSurname.Size = new Size(260, 27);
			TBSurname.TabIndex = 0;
			//
			// label1
			//
			label1.AutoSize = true;
			label1.Location = new Point(24, 106);
			label1.Name = "label1";
			label1.Size = new Size(55, 20);
			label1.TabIndex = 12;
			label1.Text = "Name*";
			//
			// TBName
			//
			TBName.Location = new Point(24, 128);
			TBName.Margin = new Padding(3, 4, 3, 4);
			TBName.Name = "TBName";
			TBName.Size = new Size(260, 27);
			TBName.TabIndex = 1;
			//
			// label3
			//
			label3.AutoSize = true;
			label3.Location = new Point(24, 164);
			label3.Name = "label3";
			label3.Size = new Size(82, 20);
			label3.TabIndex = 14;
			label3.Text = "Patronymic";
			//
			// TBPatronymic
			//
			TBPatronymic.Location = new Point(24, 186);
			TBPatronymic.Margin = new Padding(3, 4, 3, 4);
			TBPatronymic.Name = "TBPatronymic";
			TBPatronymic.Size = new Size(260, 27);
			TBPatronymic.TabIndex = 2;
			//
			// label6
			//
			label6.AutoSize = true;
			label6.Location = new Point(24, 222);
			label6.Name = "label6";
			label6.Size = new Size(70, 20);
			label6.TabIndex = 17;
			label6.Text = "Birthday*";
			//
			// MTBBirthday
			//
			MTBBirthday.Culture = new System.Globalization.CultureInfo("");
			MTBBirthday.Location = new Point(24, 244);
			MTBBirthday.Margin = new Padding(3, 4, 3, 4);
			MTBBirthday.Mask = "00.00.0000";
			MTBBirthday.Name = "MTBBirthday";
			MTBBirthday.Size = new Size(140, 27);
			MTBBirthday.TabIndex = 3;
			MTBBirthday.ValidatingType = typeof(DateTime);
			//
			// label5
			//
			label5.AutoSize = true;
			label5.Location = new Point(180, 222);
			label5.Name = "label5";
			label5.Size = new Size(36, 20);
			label5.TabIndex = 16;
			label5.Text = "Age";
			//
			// TBAge
			//
			TBAge.Location = new Point(180, 244);
			TBAge.Margin = new Padding(3, 4, 3, 4);
			TBAge.Name = "TBAge";
			TBAge.ReadOnly = true;
			TBAge.TabStop = false;
			TBAge.Size = new Size(104, 27);
			TBAge.TabIndex = 4;
			//
			// label4
			//
			label4.AutoSize = true;
			label4.Location = new Point(320, 48);
			label4.Name = "label4";
			label4.Size = new Size(34, 20);
			label4.TabIndex = 15;
			label4.Text = "IIN*";
			//
			// MTBIIN
			//
			MTBIIN.Location = new Point(320, 70);
			MTBIIN.Margin = new Padding(3, 4, 3, 4);
			MTBIIN.Mask = "000000000000";
			MTBIIN.Name = "MTBIIN";
			MTBIIN.Size = new Size(276, 27);
			MTBIIN.TabIndex = 5;
			MTBIIN.ValidatingType = typeof(int);
			//
			// label8
			//
			label8.AutoSize = true;
			label8.Location = new Point(320, 106);
			label8.Name = "label8";
			label8.Size = new Size(55, 20);
			label8.TabIndex = 19;
			label8.Text = "Phone*";
			//
			// MTBPhoneNumber
			//
			MTBPhoneNumber.Location = new Point(320, 128);
			MTBPhoneNumber.Margin = new Padding(3, 4, 3, 4);
			MTBPhoneNumber.Mask = "+7(999) 000-0000";
			MTBPhoneNumber.Name = "MTBPhoneNumber";
			MTBPhoneNumber.Size = new Size(276, 27);
			MTBPhoneNumber.TabIndex = 6;
			//
			// label7
			//
			label7.AutoSize = true;
			label7.Location = new Point(24, 290);
			label7.Name = "label7";
			label7.Size = new Size(67, 20);
			label7.TabIndex = 18;
			label7.Text = "Address*";
			//
			// MTBAdress
			//
			MTBAdress.Location = new Point(24, 312);
			MTBAdress.Margin = new Padding(3, 4, 3, 4);
			MTBAdress.Name = "MTBAdress";
			MTBAdress.Size = new Size(572, 27);
			MTBAdress.TabIndex = 7;
			//
			// BCancel
			//
			BCancel.Location = new Point(376, 368);
			BCancel.Name = "BCancel";
			BCancel.Size = new Size(100, 40);
			BCancel.TabIndex = 10;
			BCancel.Text = "Cancel";
			BCancel.UseVisualStyleBackColor = true;
			BCancel.Click += BCancel_Click;
			//
			// BAddMember
			//
			BAddMember.Location = new Point(486, 368);
			BAddMember.Name = "BAddMember";
			BAddMember.Size = new Size(110, 40);
			BAddMember.TabIndex = 8;
			BAddMember.Text = "Add";
			BAddMember.UseVisualStyleBackColor = true;
			BAddMember.Click += BAddMember_Click;
			//
			// BUpdateMember
			//
			BUpdateMember.Location = new Point(486, 368);
			BUpdateMember.Name = "BUpdateMember";
			BUpdateMember.Size = new Size(110, 40);
			BUpdateMember.TabIndex = 9;
			BUpdateMember.Text = "Save";
			BUpdateMember.UseVisualStyleBackColor = true;
			BUpdateMember.Click += BUpdateMember_Click;
			//
			// FaddEdit_prop
			//
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			CancelButton = BCancel;
			ClientSize = new Size(620, 428);
			Controls.Add(lblPersonal);
			Controls.Add(lblContacts);
			Controls.Add(label2);
			Controls.Add(TBSurname);
			Controls.Add(label1);
			Controls.Add(TBName);
			Controls.Add(label3);
			Controls.Add(TBPatronymic);
			Controls.Add(label6);
			Controls.Add(MTBBirthday);
			Controls.Add(label5);
			Controls.Add(TBAge);
			Controls.Add(label4);
			Controls.Add(MTBIIN);
			Controls.Add(label8);
			Controls.Add(MTBPhoneNumber);
			Controls.Add(label7);
			Controls.Add(MTBAdress);
			Controls.Add(BCancel);
			Controls.Add(BAddMember);
			Controls.Add(BUpdateMember);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Margin = new Padding(3, 4, 3, 4);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "FaddEdit_prop";
			StartPosition = FormStartPosition.CenterParent;
			FormClosing += FaddEdit_prop_FormClosing;
			Load += FaddEdit_prop_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private Label lblPersonal;
		private Label lblContacts;
		private TextBox TBSurname;
        private TextBox TBPatronymic;
        private TextBox TBAge;
        private Button BCancel;
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
	}
}
