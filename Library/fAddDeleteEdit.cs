using Microsoft.EntityFrameworkCore;
using System;
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
    public partial class fAddDeleteEdit : Form
    {
        byte[] photo { get; set; }
        public fAddDeleteEdit()
        {
            fMembers.OnfAddDeleteEditCreatedEvent += editRequested;
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
            //TODO strict control of textbox text lenght (example Name=75 cause column has type varchar(75))
            //check "fields" for null
            if (tbName.Text != null && tbSurname.Text != null && mtbBirthday.Text != null && mtbAdress.Text != null
                && mtbPhoneNumber.Text != null && mtbIIN.Text != null && pbPhoto.Image != null)
            //
            {
                //check corresponding to patterns 
                if (RegexController.Check(tbName.Text, tbName) && RegexController.Check(tbSurname.Text, tbSurname)
                    && RegexController.Check(mtbBirthday.Text, mtbBirthday) &&
                    RegexController.Check(mtbAdress.Text, mtbAdress) && RegexController.Check(mtbPhoneNumber.Text, mtbPhoneNumber))
                {
                    if (tbPatronymic.Text != null)
                    {
                        if (RegexController.Check(tbPatronymic.Text, tbPatronymic))
                        {
                            //
                            //Add object to dataBase(Member entity)
                            using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                            {
                                Member member = new Member(tbName.Text, tbSurname.Text, DateTime.Parse(mtbBirthday.Text),
                                    mtbAdress.Text, Convert.ToInt64(mtbIIN.Text), mtbPhoneNumber.Text, photo);
                                db.Members.Add(member);
                                int number = db.SaveChanges();
                                //if member were added then question user for another one
                                if (number == 1)
                                {
                                    DialogResult result = MessageBox.Show("Add another one?", "Member succesfully added",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                                    if (result == DialogResult.No || result == DialogResult.Abort)
                                    {
                                        this.Close();
                                    }
                                    else
                                    {
                                        pbPhoto.Image = null;
                                        foreach (Control control in this.Controls)
                                        {
                                            if (control is TextBox textbox)
                                            {
                                                textbox.Text = "";
                                            }
                                            if (control is MaskedTextBox mtb)
                                            {
                                                mtb.Text = "";
                                            }
                                        }
                                    }
                                }
                            }
                            //
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("fill in the empty requiered(*) fields");
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
            /*if (sender is TextBoxBase tb)
            {
                if (tb.SelectionStart==tb.TextLength)
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
            }  */ //TODO think about it
        }

        private void fAddDeleteEdit_Load(object sender, EventArgs e)
        {

        }
        internal void editRequested(MemberEventArgs e) //TODO this method should also handle remove
        {
            bAddMember.Enabled = false;
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
                        pbPhoto.Image = null;
                    }
                }
            }
        }

        private void bDeleteMember_Click(object sender, EventArgs e)
        {
            long number;
            if (mtbIIN.Text != null && long.TryParse(mtbIIN.Text, out number))
            {
                Task deleteMember = new TaskFactory().StartNew(new Action(() =>
                {
                    using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                    {
                        Member memberToDelete = db.Members.FirstOrDefault(m => m.IIN == Convert.ToInt64(mtbIIN.Text));
                        db.Members.Attach(memberToDelete);
                        db.Members.Remove(memberToDelete);
                        DialogResult result = MessageBox.Show("Are you sure to remove?", "Removing member", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            if (db.SaveChanges() > 0)
                            {
                                MessageBox.Show("Member succesfully removed");
                                this.Invoke(new Action(() =>
                                {
                                    this.Close();
                                }));
                            }
                        }
                    }
                }));
            }
            else
            {
                MessageBox.Show("Cannot delete this member, try later or communicate your system admin");
            }
        }
    }
}
