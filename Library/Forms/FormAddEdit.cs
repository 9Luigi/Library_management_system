using Library.Controllers;
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
			PhotoAsBytes = photo is Image ? PictureController.ImageToByteConvert(photo) : PictureController.ImageToByteConvert(Resources.NoImage); //convert photo to bytes array for transit to database
		}

		private async void BAddMember_Click(object sender, EventArgs e)
		{
			if (CheckFieldsBeforeAction())
			{
				await ActionWithMember("CREATE");
			}
		}
		private void TextBoxBase_OnFocusEnter(object sender, EventArgs e)
		{

		}
		private void TextBoxBase_OnClick(object sender, EventArgs e)
		{

		}
		internal async void ActionRequested(MemberEventArgs e)
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
							MemberToEdit = await db.Members.FirstOrDefaultAsync(m => m.IIN == e.IIN);
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
						if (imageByte == null) { pbPhoto.Image = Resources.NoImage; return; }
						var photo = PictureController.ConvertByteToImage(imageByte);
						if (photo is Image)
						{
							pbPhoto.Image = photo;
							return;
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
		private async Task ActionWithMember(string operation)
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
					await db.AddAsync(createdMember);
					int answer = await db.SaveChangesAsync();
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
						else MessageBox.Show($"Cannot add {createdMember.Name} {createdMember.Surname} try again later");
					}
					catch (DbUpdateException)
					{
						MessageBox.Show("While you were editing this member, his data was updated or delete, try again please");
					}
					break;
				case "UPDATE":
					BAddMember.Enabled = false; //TODO change age by IIN deconstruction
					if (!CheckAndMarkChanges(db, MemberToEdit!, TBName.Text, TBSurname.Text, CheckIfHasPatronymic(TBPatronymic.Text),
					  DateTime.Parse(MTBBirthday.Text), byte.Parse(TBAge.Text), MTBAdress.Text, MTBPhoneNumber.Text,
					  PictureController.ImageToByteConvert(pbPhoto.Image)))
					{
						db.Entry(MemberToEdit).State = EntityState.Unchanged;
						MessageBox.Show("You did't change member's fields");
						return;
					}
					try
					{
						MemberToEdit!.Name = TBName.Text;
						MemberToEdit.Surname = TBSurname.Text;
						MemberToEdit.BirthDay = DateTime.Parse(MTBBirthday.Text);
						MemberToEdit.Age = byte.Parse(TBAge.Text);
						MemberToEdit.Adress = MTBAdress.Text;
						MemberToEdit.PhoneNumber = MTBPhoneNumber.Text;
						MemberToEdit.Photo = PictureController.ImageToByteConvert(pbPhoto.Image!);//null not possible?
						MemberToEdit.Patronymic = CheckIfHasPatronymic(TBPatronymic.Text);

						int result = await db.SaveChangesAsync();

						if (result == 1)
						{
							MessageBox.Show($"{MemberToEdit.Name} {MemberToEdit.Surname} updated successful");
							var closeDialog = MessageBox.Show($"{MemberToEdit.Name} {MemberToEdit.Surname} updated successfully. Close the form?", "Update Successful", MessageBoxButtons.YesNo);
							if (closeDialog == DialogResult.Yes)
							{
								Close();
							}
						}
						else
						{
							MessageBox.Show("You don't change any data, change or cancel please");
						}
					}
					catch (DbUpdateException ex)
					{
						MessageBox.Show($"Database update error: {ex.Message}");
						db.Entry(MemberToEdit).Reload();
					}
					catch (Exception ex)
					{
						MessageBox.Show($"An error occurred: {ex.Message}");
					}
					break;
			}
		}
		private bool CheckAndMarkChanges(DbContext db, Member member, string name, string surname, string patronymic, DateTime birthDay, byte age, string address, string phoneNumber, byte[]? photo)
		{
			bool hasChanged = false;

			if (!string.Equals(member.Name?.Trim(), name?.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			if (!string.Equals(member.Surname?.Trim(), surname?.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			if (!string.Equals(member.Patronymic?.Trim(), patronymic?.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			if (member.BirthDay.Date != birthDay.Date)
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			if (member.Age != age)
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			if (!string.Equals(member.Adress?.Trim(), address?.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			if (!string.Equals(member.PhoneNumber?.Trim(), phoneNumber?.Trim(), StringComparison.OrdinalIgnoreCase))
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			if ((member.Photo == null && photo != null) || (member.Photo != null && !member.Photo.SequenceEqual(photo)))
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			return hasChanged;
		}

		private async void BUpdateMember_Click(object sender, EventArgs e)
		{
			if (CheckFieldsBeforeAction())
			{
				await ActionWithMember("UPDATE");
			}
		}
	}
}


