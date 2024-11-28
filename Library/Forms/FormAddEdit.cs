using Library.Controllers;
using Library.Controllers.PictureController;
using Library.Models;
using Library.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Globalization;
using static Library.FormMembers;

namespace Library
{
	/// <summary>
	/// Form for adding or editing member properties in the library system.
	/// </summary>
	public partial class FaddEdit_prop : Form
	{
		/// <summary>
		/// A byte array that holds the photo of the member in the form of image bytes.
		/// </summary>
		byte[]? PhotoAsBytes { get; set; }
		/// <summary>
		/// A property to store the member object being edited, if any.
		/// </summary>
		internal Member? MemberToEdit { get; set; }
		private readonly Repository<Member> _memberRepository;
		/// <summary>
		/// Constructor that initializes the form and subscribes to the MemberCreateOrUpdateEvent event.
		/// Sets the default photo image.
		/// </summary>
		public FaddEdit_prop()
		{
			MemberCreateOrUpdateEvent += ActionRequested; // Subscribe to event, event is invoked on update/create calls
			InitializeComponent();
			pbPhoto.Image = Properties.Resources.NoImage;

			var dbContext = new LibraryContextForEFcore();
			_memberRepository = new(dbContext);

		}


		/// <summary>
		/// Handles the event triggered when a photo is selected by the user.
		/// Updates the photo display and stores the image as bytes.
		/// </summary>
		private void BSelectPhoto_Click(object sender, EventArgs e)
		{
			var photo = PictureController.GetImageFromFile();
			// Set photo on member's photo in picture box relying on photo or default photo if error
			if (photo == null || photo is not Image)
			{
				pbPhoto.Image = Resources.NoImage;
				PhotoAsBytes = PictureController.ImageToByteConvert(Resources.NoImage);
			}
			else
			{
				pbPhoto.Image = (Image)photo;
				PhotoAsBytes = PictureController.ImageToByteConvert((Image)photo);
			}
		}

		/// <summary>
		/// Handles the event when the Add Member button is clicked.
		/// Performs validation and calls the action to create a new member.
		/// </summary>
		private async void BAddMember_Click(object sender, EventArgs e)
		{
			if (CheckFieldsBeforeAction())
			{
				await ActionWithMember("CREATE");
			}
		}

		/// <summary>
		/// Handles the focus enter event for the textboxes (not implemented).
		/// </summary>
		private void TextBoxBase_OnFocusEnter(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the click event for textboxes (not implemented).
		/// </summary>
		private void TextBoxBase_OnClick(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Handles the event when a create or update action is requested.
		/// Based on the action, it either prepares the form for editing or creating a new member.
		/// </summary>
		internal async void ActionRequested(MemberEventArgs e)
		{
			switch (e.Action)
			{
				case "EDIT":
					BUpdateMember.Enabled = true;
					BAddMember.Enabled = false;
					MTBIIN.Enabled = false;
					try
					{
						MemberToEdit = await _memberRepository.GetByIdAsync(e.IIN);
						_memberRepository._dbContext.Attach(MemberToEdit);
						if (MemberToEdit != null)
						{
							FillMemberData(MemberToEdit);
						}
						else
						{
							MessageBox.Show("Cannot load data, probably member was deleted by another employee while you edit, try again please");
						}
					}
					catch (Exception)
					{
						MessageBox.Show("An error occurred while loading member data.");
					}
					break;
				case "CREATE":
					MTBIIN.Enabled = true;
					BUpdateMember.Enabled = false;
					BAddMember.Enabled = true;
					ResetForm();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Checks that the required fields are filled and validates them using Regex.
		/// </summary>
		/// <returns>True if all fields are valid, otherwise false.</returns>
		private bool CheckFieldsBeforeAction()
		{
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
				if (string.IsNullOrEmpty(TBPatronymic.Text) || TBPatronymic.Text == "None") return true;
				else if (RegexController.Check(TBPatronymic.Text, TBPatronymic)) return true;
				else return false;
			}
			else return false;
		}

		/// <summary>
		/// Checks if the patronymic field has value or returns "None" if empty.
		/// </summary>
		/// <param name="patronymic">Patronymic to check.</param>
		/// <returns>The patronymic value or "None" if empty.</returns>
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

		/// <summary>
		/// Creates or updates the member based on the operation (CREATE or UPDATE).
		/// </summary>
		/// <param name="operation">The operation type (CREATE or UPDATE).</param>
		private async Task ActionWithMember(string operation)
		{
			switch (operation)
			{
				case "CREATE":
					Member createdMember = new
						(
							TBName.Text,
							TBSurname.Text,
							DateTime.ParseExact(MTBBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture),
							MTBAdress.Text,
							Convert.ToInt64(MTBIIN.Text), //TODO check for duplicates cause it's primary key
							MTBPhoneNumber.Text,
							PhotoAsBytes!,
							CheckIfHasPatronymic(TBPatronymic.Text)
						);
					try
					{
						if (await _memberRepository.AddAsync(createdMember))
						{
							DialogResult result = MessageBox.Show
								(
									"Do you want to add another one?",
									$"{createdMember.Name} {createdMember.Surname} added successfully",
									MessageBoxButtons.YesNo
								);
							if (result == DialogResult.Yes)
							{
								ResetForm();
							}
							else
							{
								this.Close();
							}
						}
						else MessageBox.Show($"Cannot add {createdMember.Name} {createdMember.Surname} try again later. Inner exception: {{ex.InnerException?.Message ?? \"None\"}}\"");
					}
					catch (DbUpdateException ex)
					{
						MessageBox.Show($"Error updating database: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "None"}");
					}
					catch (Exception ex)
					{
						MessageBox.Show($"An unexpected error occurred: {ex.Message}. Please contact support. Inner exception: {{ex.InnerException?.Message ?? \"None\"}}\"");
					}
					break;

				case "UPDATE":
					// Directly update the object's properties
					MemberToEdit.Name = TBName.Text;
					MemberToEdit.Surname = TBSurname.Text;
					MemberToEdit.Patronymic = CheckIfHasPatronymic(TBPatronymic.Text);
					MemberToEdit.BirthDay = DateTime.ParseExact(MTBBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
					MemberToEdit.Age = byte.Parse(TBAge.Text);
					MemberToEdit.Adress = MTBAdress.Text;
					MemberToEdit.PhoneNumber = MTBPhoneNumber.Text;
					MemberToEdit.Photo = PictureController.ImageToByteConvert(pbPhoto.Image);

					// Save the changes to the database
					bool isUpdated = await _memberRepository.UpdateAttachedAsync(MemberToEdit);///////////////////////

					MessageBox.Show(_memberRepository._dbContext.Entry(MemberToEdit).State.ToString()); // check the entity's state
					if (!isUpdated)
					{
						MessageBox.Show("You didn't change member's fields");
						return;
					}
					else
					{
						MessageBox.Show("Member's fields updated successfully");
					}

					var closeDialog = MessageBox.Show($"{MemberToEdit.Name} {MemberToEdit.Surname} updated successfully. Close the form?", "Update Successful", MessageBoxButtons.YesNo);
					if (closeDialog == DialogResult.Yes)
					{
						Close();
					}
					else
					{

						FillMemberData(MemberToEdit);
						if (MemberToEdit.Photo == null) return; //TODO message and log
						pbPhoto.Image = PictureController.ConvertByteToImage(MemberToEdit.Photo);
						// Focus on the first field
						TBName.Focus();
					}
					break;
			}
		}
		/// <summary>
		/// Handles the event when the Update Member button is clicked.
		/// Performs validation and calls the action to update the member.
		/// </summary>
		private async void BUpdateMember_Click(object sender, EventArgs e)
		{
			if (CheckFieldsBeforeAction())
			{
				await ActionWithMember("UPDATE");
			}
		}

		/// <summary>
		/// Fills the form with data from the member object.
		/// </summary>
		/// <param name="member">The member object to populate the form fields.</param>
		private void FillMemberData(Member member)
		{
			MTBIIN.Text = member.IIN.ToString();
			TBName.Text = member.Name;
			TBSurname.Text = member.Surname;
			TBPatronymic.Text = member.Patronymic;
			MTBBirthday.Text = member.BirthDay.ToString("dd/MM/yyyy");
			TBAge.Text = member.Age.ToString();
			MTBAdress.Text = member.Adress;
			MTBPhoneNumber.Text = member.PhoneNumber;

			byte[]? imageByte = member.Photo;
			pbPhoto.Image = imageByte != null ? PictureController.ConvertByteToImage(imageByte) : Resources.NoImage;
		}

		/// <summary>
		/// Resets the form by clearing all textboxes and setting the default image for the photo.
		/// </summary>
		private void ResetForm()
		{
			TextBoxBaseController.AllTextBoxBaseOnFormClear(this);
			pictureBoxController.pictureBoxImageSetDefault(pbPhoto);
			TBName.Focus();
		}
	}
}
