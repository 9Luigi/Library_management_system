using Microsoft.EntityFrameworkCore;
using static Library.fMembers;

namespace Library
{
    public partial class fAddEdit : Form
    {
        byte[] photo { get; set; }
        public fAddEdit()
        {
            MemberCreateOrUpdateEvent += ActionRequested; //subscribe to event, event is invoked on update/create calls
            InitializeComponent();
            pbPhoto.Image = Properties.Resources.NoImage;
        }
        internal Member memberToEdit { get; set; }
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

        }
        private void TextBoxBase_OnClick(object sender, EventArgs e)
        {
            //TODO think about it
        }
        internal void ActionRequested(MemberEventArgs e)
        {//handle create/update event
            switch (e.Action)
            {
                case "EDIT":
                    bUpdateMember.Enabled = true;
                    bAddMember.Enabled = false;
                    mtbIIN.Enabled = false;
                    mtbIIN.Text = e.IIN.ToString();
                    using (LibraryContextForEFcore db = new LibraryContextForEFcore())
                    {
                        memberToEdit = db.Members.FirstOrDefault(m => m.IIN == e.IIN);
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
                    TextBoxBaseController.AllTextBoxBaseOnFormClear(this);
                    pictureBoxController.pictureBoxImageSetDefault(pbPhoto);
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
        {//check properties for null and by RegexController
            foreach (Control control in this.Controls)
            {
                if (control is TextBoxBase textBoxBase)
                {
                    if (TextBoxBaseController.checkTextBoxBaseTextOnNull(textBoxBase))
                    {
                        return false;
                    }
                } 
                else return true;
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
        void actionWithMember(string operation)
        {
            using (LibraryContextForEFcore db = new LibraryContextForEFcore())
            {
                switch (operation)
                {
                    case "CREATE":
                        Member createdMember = MemberRepository.CreateMember
                            (
                            tbName.Text,
                                        tbSurname.Text,
                                        DateTime.Parse(mtbBirthday.Text),
                                        mtbAdress.Text,
                                        Convert.ToInt64(mtbIIN.Text), //TODO better parse long?
                                        mtbPhoneNumber.Text,
                                        photo,
                                        checkIfHasPatronymic(tbPatronymic.Text)
                            );
                        try
                        {
                            if (MemberRepository.AddMemberToDataBase(db, createdMember))
                            {
                                DialogResult result = MessageBox.Show
                                    (
                                        "Do you want to add another one?",
                                        $"{createdMember.Name} {createdMember.Surname} added succesfully",
                                        MessageBoxButtons.YesNo
                                    );
                                if (result == DialogResult.Yes)
                                {
                                    TextBoxBaseController.AllTextBoxBaseOnFormClear(this);
                                    pictureBoxController.pictureBoxImageSetDefault(pbPhoto);
                                }
                                else
                                {
                                    this.Close();
                                }
                            }
                        }
                        catch (DbUpdateException)
                        {
                            MessageBox.Show("Cannot save data, check internet connection and try later please");
                        }
                        break;
                    case "UPDATE":
                        bAddMember.Enabled = false;
                        long IIN;
                        long.TryParse(mtbIIN.Text, out IIN); //TODO what if cannot parse?, can it be?
                        db.Members.Attach(memberToEdit); //TODO parallel or check if already deleted
                        memberToEdit.Name = tbName.Text;
                        memberToEdit.Surname = tbSurname.Text;
                        memberToEdit.BirthDay = DateTime.Parse(mtbBirthday.Text);
                        memberToEdit.Adress = mtbAdress.Text;
                        memberToEdit.PhoneNumber = mtbPhoneNumber.Text;
                        memberToEdit.Photo = PictureController.ImageToByteConvert(pbPhoto.Image);//TODO Check null
                        memberToEdit.Patronymic = checkIfHasPatronymic(tbPatronymic.Text);
                        try
                        {
                            int number = db.SaveChanges();//TODO check values on change before call savechanges
                            //TODO check fields, if their values did't change then don't call SaveChages
                            if (number == 1)
                            {
                                MessageBox.Show($"{memberToEdit.Name} {memberToEdit.Surname} updated successful");
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("You don't change any data, change or cancel please");
                            }
                        }
                        /*catch (DBConcurrencyException)
                        {
                            MessageBox.Show("While you were editing this member, his data was updated, try again please");
                            Close();
                        }*/
                        catch (DbUpdateException)
                        {
                            MessageBox.Show("While you were editing this member, his data was updated, try again please");
                            Close();
                        }
                        break;
                }
            }
        }
    }
}


