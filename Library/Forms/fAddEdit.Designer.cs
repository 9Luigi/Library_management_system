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
            this.TBSurname = new System.Windows.Forms.TextBox();
            this.TBPatronymic = new System.Windows.Forms.TextBox();
            this.TBAge = new System.Windows.Forms.TextBox();
            this.pbPhoto = new System.Windows.Forms.PictureBox();
            this.BSelectPhoto = new System.Windows.Forms.Button();
            this.BAddMember = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.MTBPhoneNumber = new System.Windows.Forms.MaskedTextBox();
            this.MTBBirthday = new System.Windows.Forms.MaskedTextBox();
            this.MTBIIN = new System.Windows.Forms.MaskedTextBox();
            this.TBName = new System.Windows.Forms.TextBox();
            this.MTBAdress = new System.Windows.Forms.MaskedTextBox();
            this.BUpdateMember = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // TBSurname
            // 
            this.TBSurname.Location = new System.Drawing.Point(12, 50);
            this.TBSurname.Name = "TBSurname";
            this.TBSurname.Size = new System.Drawing.Size(100, 23);
            this.TBSurname.TabIndex = 1;
            this.TBSurname.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.TBSurname.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // TBPatronymic
            // 
            this.TBPatronymic.Location = new System.Drawing.Point(12, 88);
            this.TBPatronymic.Name = "TBPatronymic";
            this.TBPatronymic.Size = new System.Drawing.Size(100, 23);
            this.TBPatronymic.TabIndex = 2;
            this.TBPatronymic.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.TBPatronymic.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // TBAge
            // 
            this.TBAge.Enabled = false;
            this.TBAge.Location = new System.Drawing.Point(12, 164);
            this.TBAge.Name = "TBAge";
            this.TBAge.Size = new System.Drawing.Size(100, 23);
            this.TBAge.TabIndex = 4;
            this.TBAge.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.TBAge.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
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
            // BSelectPhoto
            // 
            this.BSelectPhoto.Location = new System.Drawing.Point(314, 307);
            this.BSelectPhoto.Name = "BSelectPhoto";
            this.BSelectPhoto.Size = new System.Drawing.Size(101, 23);
            this.BSelectPhoto.TabIndex = 8;
            this.BSelectPhoto.Text = "Select photo*";
            this.BSelectPhoto.UseVisualStyleBackColor = true;
            this.BSelectPhoto.Click += new System.EventHandler(this.BSelectPhoto_Click);
            // 
            // BAddMember
            // 
            this.BAddMember.Location = new System.Drawing.Point(12, 336);
            this.BAddMember.Name = "BAddMember";
            this.BAddMember.Size = new System.Drawing.Size(119, 46);
            this.BAddMember.TabIndex = 9;
            this.BAddMember.Text = "Add member";
            this.BAddMember.UseVisualStyleBackColor = true;
            this.BAddMember.Click += new System.EventHandler(this.BAddMember_Click);
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
            // MTBPhoneNumber
            // 
            this.MTBPhoneNumber.Location = new System.Drawing.Point(12, 278);
            this.MTBPhoneNumber.Mask = "+7(999) 000-0000";
            this.MTBPhoneNumber.Name = "MTBPhoneNumber";
            this.MTBPhoneNumber.Size = new System.Drawing.Size(100, 23);
            this.MTBPhoneNumber.TabIndex = 7;
            this.MTBPhoneNumber.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.MTBPhoneNumber.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // MTBBirthday
            // 
            this.MTBBirthday.Location = new System.Drawing.Point(12, 202);
            this.MTBBirthday.Mask = "00/00/0000";
            this.MTBBirthday.Name = "MTBBirthday";
            this.MTBBirthday.Size = new System.Drawing.Size(100, 23);
            this.MTBBirthday.TabIndex = 5;
            this.MTBBirthday.ValidatingType = typeof(System.DateTime);
            this.MTBBirthday.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.MTBBirthday.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // MTBIIN
            // 
            this.MTBIIN.Location = new System.Drawing.Point(12, 126);
            this.MTBIIN.Mask = "000000000000";
            this.MTBIIN.Name = "MTBIIN";
            this.MTBIIN.Size = new System.Drawing.Size(100, 23);
            this.MTBIIN.TabIndex = 3;
            this.MTBIIN.ValidatingType = typeof(int);
            this.MTBIIN.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.MTBIIN.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // TBName
            // 
            this.TBName.Location = new System.Drawing.Point(12, 12);
            this.TBName.Name = "TBName";
            this.TBName.Size = new System.Drawing.Size(100, 23);
            this.TBName.TabIndex = 0;
            this.TBName.Click += new System.EventHandler(this.TextBoxBase_OnClick);
            this.TBName.Enter += new System.EventHandler(this.TextBoxBase_OnFocusEnter);
            // 
            // MTBAdress
            // 
            this.MTBAdress.Location = new System.Drawing.Point(12, 240);
            this.MTBAdress.Name = "MTBAdress";
            this.MTBAdress.Size = new System.Drawing.Size(100, 23);
            this.MTBAdress.TabIndex = 6;
            // 
            // BUpdateMember
            // 
            this.BUpdateMember.Location = new System.Drawing.Point(212, 336);
            this.BUpdateMember.Name = "BUpdateMember";
            this.BUpdateMember.Size = new System.Drawing.Size(119, 46);
            this.BUpdateMember.TabIndex = 10;
            this.BUpdateMember.Text = "Update member";
            this.BUpdateMember.UseVisualStyleBackColor = true;
            this.BUpdateMember.Click += new System.EventHandler(this.BUpdateMember_Click);
            // 
            // FaddEdit_prop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 394);
            this.Controls.Add(this.BUpdateMember);
            this.Controls.Add(this.MTBAdress);
            this.Controls.Add(this.MTBIIN);
            this.Controls.Add(this.MTBBirthday);
            this.Controls.Add(this.MTBPhoneNumber);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BAddMember);
            this.Controls.Add(this.BSelectPhoto);
            this.Controls.Add(this.pbPhoto);
            this.Controls.Add(this.TBAge);
            this.Controls.Add(this.TBPatronymic);
            this.Controls.Add(this.TBSurname);
            this.Controls.Add(this.TBName);
            this.Name = "FaddEdit_prop";
            ((System.ComponentModel.ISupportInitialize)(this.pbPhoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}