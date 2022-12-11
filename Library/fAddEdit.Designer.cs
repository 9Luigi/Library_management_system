namespace Library
{
    partial class fAddEdit
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
            this.tbSurname = new System.Windows.Forms.TextBox();
            this.tbPatronymic = new System.Windows.Forms.TextBox();
            this.tbAge = new System.Windows.Forms.TextBox();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.bSelectPhoto = new System.Windows.Forms.Button();
            this.bAddMember = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.mtbPhoneNumber = new System.Windows.Forms.MaskedTextBox();
            this.mtbBirthday = new System.Windows.Forms.MaskedTextBox();
            this.mtbIIN = new System.Windows.Forms.MaskedTextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.mtbAdress = new System.Windows.Forms.MaskedTextBox();
            this.bUpdateMember = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // tbSurname
            // 
            this.tbSurname.Location = new System.Drawing.Point(12, 50);
            this.tbSurname.Name = "tbSurname";
            this.tbSurname.Size = new System.Drawing.Size(100, 23);
            this.tbSurname.TabIndex = 1;
            this.tbSurname.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.tbSurname.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // tbPatronymic
            // 
            this.tbPatronymic.Location = new System.Drawing.Point(12, 88);
            this.tbPatronymic.Name = "tbPatronymic";
            this.tbPatronymic.Size = new System.Drawing.Size(100, 23);
            this.tbPatronymic.TabIndex = 2;
            this.tbPatronymic.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.tbPatronymic.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // tbAge
            // 
            this.tbAge.Enabled = false;
            this.tbAge.Location = new System.Drawing.Point(12, 164);
            this.tbAge.Name = "tbAge";
            this.tbAge.Size = new System.Drawing.Size(100, 23);
            this.tbAge.TabIndex = 4;
            this.tbAge.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.tbAge.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // pbPhoto
            // 
            this.pbPhoto.Location = new System.Drawing.Point(230, 12);
            this.pbPhoto.Name = "pbPhoto";
            this.pbPhoto.Size = new System.Drawing.Size(270, 289);
            this.pbPhoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPhoto.TabIndex = 9;
            this.pbPhoto.TabStop = false;
            // 
            // bSelectPhoto
            // 
            this.bSelectPhoto.Location = new System.Drawing.Point(314, 307);
            this.bSelectPhoto.Name = "bSelectPhoto";
            this.bSelectPhoto.Size = new System.Drawing.Size(101, 23);
            this.bSelectPhoto.TabIndex = 8;
            this.bSelectPhoto.Text = "Select photo*";
            this.bSelectPhoto.UseVisualStyleBackColor = true;
            this.bSelectPhoto.Click += new System.EventHandler(this.bSelectPhoto_Click);
            // 
            // bAddMember
            // 
            this.bAddMember.Location = new System.Drawing.Point(12, 336);
            this.bAddMember.Name = "bAddMember";
            this.bAddMember.Size = new System.Drawing.Size(119, 46);
            this.bAddMember.TabIndex = 9;
            this.bAddMember.Text = "Add member";
            this.bAddMember.UseVisualStyleBackColor = true;
            this.bAddMember.Click += new System.EventHandler(this.bAddMember_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(118, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "Name*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "Surname*";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(118, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Patronymic";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "IIN*";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(118, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "Age";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(118, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 15);
            this.label6.TabIndex = 17;
            this.label6.Text = "Birthday*";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(118, 243);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 15);
            this.label7.TabIndex = 18;
            this.label7.Text = "Adress";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(118, 281);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 15);
            this.label8.TabIndex = 19;
            this.label8.Text = "Phone number";
            // 
            // mtbPhoneNumber
            // 
            this.mtbPhoneNumber.Location = new System.Drawing.Point(12, 278);
            this.mtbPhoneNumber.Mask = "+7(999) 000-0000";
            this.mtbPhoneNumber.Name = "mtbPhoneNumber";
            this.mtbPhoneNumber.Size = new System.Drawing.Size(100, 23);
            this.mtbPhoneNumber.TabIndex = 7;
            this.mtbPhoneNumber.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.mtbPhoneNumber.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // mtbBirthday
            // 
            this.mtbBirthday.Location = new System.Drawing.Point(12, 202);
            this.mtbBirthday.Mask = "00/00/0000";
            this.mtbBirthday.Name = "mtbBirthday";
            this.mtbBirthday.Size = new System.Drawing.Size(100, 23);
            this.mtbBirthday.TabIndex = 5;
            this.mtbBirthday.ValidatingType = typeof(System.DateTime);
            this.mtbBirthday.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.mtbBirthday.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // mtbIIN
            // 
            this.mtbIIN.Location = new System.Drawing.Point(12, 126);
            this.mtbIIN.Mask = "000000000000";
            this.mtbIIN.Name = "mtbIIN";
            this.mtbIIN.Size = new System.Drawing.Size(100, 23);
            this.mtbIIN.TabIndex = 3;
            this.mtbIIN.ValidatingType = typeof(int);
            this.mtbIIN.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.mtbIIN.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(12, 12);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(100, 23);
            this.tbName.TabIndex = 0;
            this.tbName.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.tbName.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // mtbAdress
            // 
            this.mtbAdress.Location = new System.Drawing.Point(12, 240);
            this.mtbAdress.Name = "mtbAdress";
            this.mtbAdress.Size = new System.Drawing.Size(100, 23);
            this.mtbAdress.TabIndex = 6;
            // 
            // bUpdateMember
            // 
            this.bUpdateMember.Location = new System.Drawing.Point(212, 336);
            this.bUpdateMember.Name = "bUpdateMember";
            this.bUpdateMember.Size = new System.Drawing.Size(119, 46);
            this.bUpdateMember.TabIndex = 10;
            this.bUpdateMember.Text = "Update member";
            this.bUpdateMember.UseVisualStyleBackColor = true;
            this.bUpdateMember.Click += new System.EventHandler(this.bUpdateMember_Click);
            // 
            // fAddDeleteEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 394);
            this.Controls.Add(this.bUpdateMember);
            this.Controls.Add(this.mtbAdress);
            this.Controls.Add(this.mtbIIN);
            this.Controls.Add(this.mtbBirthday);
            this.Controls.Add(this.mtbPhoneNumber);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bAddMember);
            this.Controls.Add(this.bSelectPhoto);
            this.Controls.Add(this.pbPhoto);
            this.Controls.Add(this.tbAge);
            this.Controls.Add(this.tbPatronymic);
            this.Controls.Add(this.tbSurname);
            this.Controls.Add(this.tbName);
            this.Name = "fAddDeleteEdit";
            this.Load += new System.EventHandler(this.fAddDeleteEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox tbSurname;
        private TextBox tbPatronymic;
        private TextBox tbAge;
        private PictureBox pbPhoto;
        private Button bSelectPhoto;
        private Button bAddMember;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private MaskedTextBox mtbPhoneNumber;
        private MaskedTextBox mtbBirthday;
        private MaskedTextBox mtbIIN;
        private TextBox tbName;
        private MaskedTextBox mtbAdress;
        private Button bUpdateMember;
    }
}