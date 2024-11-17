﻿using Library.Controllers;
using Library.Controllers.PictureController;
using Library.Models;
using Library.Properties;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;
using static Library.FormMembers;

namespace Library
{
    public partial class FaddEdit_prop : Form
    {
        byte[]? PhotoAsBytes { get; set; }
        public FaddEdit_prop()
        {
            MemberCreateOrUpdateEvent += ActionRequested; //subscribe to event, event is invoked on update/create calls
            InitializeComponent();
            pbPhoto.Image = Properties.Resources.NoImage;
        }
        internal Member? MemberToEdit { get; set; }
        private void BSelectPhoto_Click(object sender, EventArgs e)
        {
            var photo = PictureController.GetImageFromFile(); //opens file dialog and get image if capture success
            pbPhoto.Image = photo is Image ? photo : Resources.NoImage; //show captured photo if capture was success
			PhotoAsBytes = photo is Image ? PictureController.ImageToByteConvert(photo) : null; //convert photo to bytes array for transit to database
        }

        private void BAddMember_Click(object sender, EventArgs e)
        {
            if (CheckFieldsBeforeAction())
            {
                ActionWithMember("CREATE");
            }
        }
        private void TextBoxBase_OnFocusEnter(object sender, EventArgs e)
        {

        }
        private void TextBoxBase_OnClick(object sender, EventArgs e)
        {
            
        }
        internal void ActionRequested(MemberEventArgs e)
        {//handle create/update event
            switch (e.Action)
            {
                case "EDIT":
                    BUpdateMember.Enabled = true;
                    BAddMember.Enabled = false;
                    MTBIIN.Enabled = false;
                    MTBIIN.Text = e.IIN.ToString();

                    using (LibraryContextForEFcore db = new())
                    {
                        try
                        {
                            MemberToEdit = db.Members.FirstOrDefault(m => m.IIN == e.IIN);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Cannot load data, probably member was deleted by another employee while you edit, try again please");
                        }
                        MTBIIN.Text = MemberToEdit!.IIN.ToString();
                        TBName.Text = MemberToEdit.Name;
                        TBSurname.Text = MemberToEdit.Surname;
                        TBPatronymic.Text = MemberToEdit.Patronymic;
                        TBAge.Text = MemberToEdit.Age.ToString();
                        MTBBirthday.Text = MemberToEdit.BirthDay.ToString();
                        MTBAdress.Text = MemberToEdit.Adress;
                        MTBPhoneNumber.Text = MemberToEdit.PhoneNumber;
                        byte[]? imageByte = MemberToEdit.Photo;
                        using MemoryStream ms = new(imageByte!);
                        try
                        {
                            pbPhoto.Image = Image.FromStream(ms);
                        }
                        catch
                        {
                            pbPhoto.Image = Properties.Resources.NoImage;
                        }
                    }
                    break;
                case "CREATE":
                    BUpdateMember.Enabled = false;
                    BAddMember.Enabled = true;
                    TextBoxBaseController.AllTextBoxBaseOnFormClear(this);
                    pictureBoxController.pictureBoxImageSetDefault(pbPhoto);
                    break;
                default:
                    break;
            }
        }
        private bool CheckFieldsBeforeAction()
        {//check properties for null and by RegexController
            foreach (Control control in this.Controls)
            {
                if (control is TextBoxBase textBoxBase && !TextBoxBaseController.CheckTextBoxBaseTextOnNull(textBoxBase))
                {
                    return false;
                }
            }
            if (RegexController.Check(TBName.Text, TBName) && RegexController.Check(TBSurname.Text, TBSurname)
            && RegexController.Check(MTBBirthday.Text, MTBBirthday) &&
            RegexController.Check(MTBAdress.Text, MTBAdress) && RegexController.Check(MTBPhoneNumber.Text, MTBPhoneNumber))
            {
                if (TBPatronymic.Text == "" || TBPatronymic.Text == "None") return true;
                else if (RegexController.Check(TBPatronymic.Text, TBPatronymic)) return true;
                else return false;
            }
            else return false;
        }
        private static string CheckIfHasPatronymic(string patronymic)
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
        private void ActionWithMember(string operation)
        {
            using LibraryContextForEFcore db = new();
            switch (operation)
            {
                case "CREATE":
                    Member createdMember = new
                        (
                            TBName.Text,
                            TBSurname.Text,
                            DateTime.Parse(MTBBirthday.Text),
                            MTBAdress.Text,
                            Convert.ToInt64(MTBIIN.Text),
                            MTBPhoneNumber.Text,
							PhotoAsBytes!,
                            CheckIfHasPatronymic(TBPatronymic.Text)
                        );
                    db.Add(createdMember);
                    int answer = db.SaveChanges();
                    try
                    {
                        if (answer == 1)
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
                        else MessageBox.Show($"Cannot add {createdMember.Name} {createdMember.Surname}");
                    }
                    catch (DbUpdateException)
                    {
                        MessageBox.Show("While you were editing this member, his data was updated or delete, try again please");
                    }
                    break;
                case "UPDATE":
                    BAddMember.Enabled = false;
                    db.Attach(MemberToEdit!);
                    MemberToEdit!.Name = TBName.Text;
                    MemberToEdit.Surname = TBSurname.Text;
                    MemberToEdit.BirthDay = DateTime.Parse(MTBBirthday.Text);
                    MemberToEdit.Adress = MTBAdress.Text;
                    MemberToEdit.PhoneNumber = MTBPhoneNumber.Text;
                    MemberToEdit.Photo = pbPhoto.Image == null ? PictureController.ImageToByteConvert(pbPhoto.Image!) : null;//null not possible?
                    MemberToEdit.Patronymic = CheckIfHasPatronymic(TBPatronymic.Text);
                    try
                    {
                        int number = db.SaveChanges();//TODO check values on change before call savechanges
                                                      //TODO check fields, if their values did't change then don't call SaveChages
                        if (number == 1)
                        {
                            MessageBox.Show($"{MemberToEdit.Name} {MemberToEdit.Surname} updated successful");
                            Close();
                        }
                        else
                        {
                            MessageBox.Show("You don't change any data, change or cancel please");
                        }
                    }
                    catch (DbUpdateException)
                    {
                        MessageBox.Show("While you were editing this member, his data was updated or delete, try again with new data or close please");
                        MemberToEdit = db.Members.FirstOrDefault(m => m.IIN == Convert.ToInt64(MTBIIN.Text));
                    }
                    break;
            }
        }

        private void BUpdateMember_Click(object sender, EventArgs e)
        {
            if (CheckFieldsBeforeAction())
            {
                ActionWithMember("UPDATE");
            }
        }
    }
}


