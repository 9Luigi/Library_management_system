using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library
{
    public partial class fAddDeleteEdit : Form
    {
        byte[] photo { get; set; }
        public fAddDeleteEdit()
        {
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

        private async void bAddMember_Click(object sender, EventArgs e)
        {
            if (tbName.Text!=null && tbSurname.Text!=null && mtbBirthday.Text!=null && tbAdress.Text!=null && mtbPhoneNumber.Text!=null &&mtbIIN.Text!=null && pbPhoto.Image!=null)
            {
                using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                {
                    Member member = new Member(tbName.Text, tbSurname.Text, DateTime.Parse(mtbBirthday.Text), tbAdress.Text, Convert.ToInt64(mtbIIN.Text), Convert.ToInt64(mtbPhoneNumber.Text), photo);
                    db.Members.Add(member);
                    int number = await db.SaveChangesAsync();
                    if (number==1)
                    {
                        DialogResult result = MessageBox.Show("Add another one?","Member succesfully added",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button1);
                        if (result==DialogResult.No || result==DialogResult.Abort)
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
                            }
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
    }
}
