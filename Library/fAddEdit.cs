using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Library.fMembers;

namespace Library
{
    public partial class fAddEdit : Form
    {
        byte[] photo { get; set; }
        public fAddEdit()
        {
            Need_IIN_Event += ActionRequested;
            InitializeComponent();
        }

        private void bSelectPhoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();
            pbPhoto.Image = Image.FromFile(fd.FileName);
            photo = File.ReadAllBytes(fd.FileName);
            fd.Dispose();
        }

        private void bAddMember_Click(object sender, EventArgs e)
        {
            if (checkFieldsBeforeAction())
            {
                actionWithMember("CREATE");
            }
        }
        private void TextBoxBase_OnFocusEnter(object sender, EventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectionStart = 0;
            }
            if (sender is MaskedTextBox mtb)
            {
                mtb.SelectionStart = 0;
            }
        }
        private void TextBoxBase_OnClick(object sender, EventArgs e)
        {
            if (sender is TextBoxBase tb)
            {
                if (tb.SelectionStart == tb.TextLength)
                {
                    tb.SelectionStart = 0;
                }
            }
            if (sender is MaskedTextBox mtb)
            {
                if (mtb.SelectionStart == mtb.TextLength)
                {
                    mtb.SelectionStart = 0;
                }
            }   //TODO think about it
        }
        internal void ActionRequested(MemberEventArgs e)
        {
            switch (e.Action)
            {
                case "EDIT":
                    bUpdateMember.Enabled = true;
                    bAddMember.Enabled = false;
                    mtbIIN.Enabled = false;
                    mtbIIN.Text = e.IIN.ToString();
                    using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                    {
                        var memberToEdit = db.Members.FirstOrDefault(m => m.IIN == e.IIN);
                        mtbIIN.Text = memberToEdit.IIN.ToString();
                        tbName.Text = memberToEdit.Name;
                        tbSurname.Text = memberToEdit.Surname;
                        tbPatronymic.Text = memberToEdit.Patronymic;
                        tbAge.Text = memberToEdit.Age.ToString();
                        mtbBirthday.Text = memberToEdit.BirthDay.ToString();
                        mtbAdress.Text = memberToEdit.Adress;
                        mtbPhoneNumber.Text = memberToEdit.PhoneNumber;

                        byte[] imageByte = memberToEdit.Photo;
                        using (MemoryStream ms = new MemoryStream(imageByte))
                        {
                            try
                            {
                                pbPhoto.Image = Image.FromStream(ms);
                            }
                            catch
                            {
                                pbPhoto.Image = Properties.Resources.NoImage;
                            }
                        }
                    }
                    break;
                case "CREATE":
                    bUpdateMember.Enabled = false;
                    bAddMember.Enabled = true;
                    foreach (Control control in this.Controls)
                    {
                        if (control is TextBoxBase textBoxBase) textBoxBase.Text = "";
                        if (control is PictureBox pictureBox) pictureBox.Image = Properties.Resources.NoImage;
                    }
                    break;
                default:
                    break;
            }
        }
        private void bUpdateMember_Click(object sender, EventArgs e)
        {
            if (checkFieldsBeforeAction())
            {
                actionWithMember("UPDATE");
            }
        }
        private bool checkFieldsBeforeAction()
        {
            if (tbName.Text != null && tbSurname.Text != null && mtbBirthday.Text != null && mtbAdress.Text != null
                && mtbPhoneNumber.Text != null && mtbIIN.Text != null && pbPhoto.Image != null)
            {
                foreach (Control control in this.Controls)
                {
                    if (control is TextBox textBox && textBox.Text.Length > 75)
                    {
                        MessageBox.Show($"{control.Name} cannot be more than 75 symbols");
                        return false;
                    }
                }
                if (RegexController.Check(tbName.Text, tbName) && RegexController.Check(tbSurname.Text, tbSurname)
                && RegexController.Check(mtbBirthday.Text, mtbBirthday) &&
                RegexController.Check(mtbAdress.Text, mtbAdress) && RegexController.Check(mtbPhoneNumber.Text, mtbPhoneNumber))
                {
                    if (tbPatronymic.Text == "" || tbPatronymic.Text == "None") return true;
                    else if (RegexController.Check(tbPatronymic.Text, tbPatronymic)) return true;
                    else return false;
                }
                else return false;
            }
            else
            {
                MessageBox.Show("fill in the empty requiered(*) fields");
                return false;
            }
        }
        string checkIfHasPatronymic(string patronymic)
        {
            if (patronymic != "")
            {
                return patronymic;
            }
            else
            {
                return "None";
            }
        }
        byte[] ImageToByte(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                Bitmap bitmap = new Bitmap(img);
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }
        void actionWithMember(string operation)
        {
            using (LibraryContextForEFcore db = new LibraryContextForEFcore())
            {
                switch (operation)
                {
                    case "CREATE":
                        Member member = new Member
                                    (
                                        tbName.Text,
                                        tbSurname.Text,
                                        DateTime.Parse(mtbBirthday.Text),
                                        mtbAdress.Text,
                                        Convert.ToInt64(mtbIIN.Text), //TODO check long?
                                        mtbPhoneNumber.Text,
                                        photo,
                                        checkIfHasPatronymic(tbPatronymic.Text)
                                    );
                        db.Members.Add(member);
                        int isSuccess = db.SaveChanges();
                        if (isSuccess == 1)
                        {
                            DialogResult result = MessageBox.Show("Do you want to add another one?", $"{member.Name} {member.Surname} added succesfully", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                TextBoxBaseClear();
                                pictureBoxImageClear(pbPhoto);
                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        break;
                    case "UPDATE":
                        bAddMember.Enabled = false;
                        long IIN;
                        long.TryParse(mtbIIN.Text, out IIN); //TODO what if cannot parse?, can it be?
                        member = db.Members.SingleOrDefault(m => m.IIN == IIN);
                        member.Name = tbName.Text;
                        member.Surname = tbSurname.Text;
                        member.BirthDay = DateTime.Parse(mtbBirthday.Text);
                        member.Adress = mtbAdress.Text;
                        member.PhoneNumber = mtbPhoneNumber.Text;
                        member.Photo = ImageToByte(pbPhoto.Image);//TODO Check null
                        member.Patronymic = checkIfHasPatronymic(tbPatronymic.Text);
                        int number = db.SaveChanges();
                        if (number == 1)
                        {
                            MessageBox.Show($"{member.Name} {member.Surname} updated successful");
                            Close();
                        }
                        else
                        {
                            MessageBox.Show($"Cannot execute {operation}");
                            Close();
                        }
                        break;
                }
            }
        }
        void TextBoxBaseClear()
        {
            foreach (var control in this.Controls)
            {
                if (control is TextBoxBase tbb)
                {
                    tbb.Text = "";
                }
            }
        }
        void pictureBoxImageClear(PictureBox pictureBox)
        {
            pictureBox.Image = null;
        }
    }
}
