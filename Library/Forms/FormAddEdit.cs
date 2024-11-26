using Library.Controllers;
using Library.Controllers.PictureController;
using Library.Models;
using Library.Properties;
using Microsoft.EntityFrameworkCore;
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
		/// Constructor that initializes the form and subscribes to the MemberCreateOrUpdateEvent event.
		/// Sets the default photo image.
		/// </summary>
		public FaddEdit_prop()
		{
			MemberCreateOrUpdateEvent += ActionRequested; // Subscribe to event, event is invoked on update/create calls
			InitializeComponent();
			pbPhoto.Image = Properties.Resources.NoImage;
		}

		/// <summary>
		/// A property to store the member object being edited, if any.
		/// </summary>
		internal Member? MemberToEdit { get; set; }

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
						using LibraryContextForEFcore db = new();
						MemberToEdit = await db.Members.FirstOrDefaultAsync(m => m.IIN == e.IIN);
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
			using LibraryContextForEFcore db = new();
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
					await db.AddAsync(createdMember);

					try
					{
						int answer = await db.SaveChangesAsync();
						if (answer == 1)
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
						else MessageBox.Show($"Cannot add {createdMember.Name} {createdMember.Surname} try again later");
					}
					catch (DbUpdateException ex)
					{
						MessageBox.Show($"Error updating database: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "None"}");
					}
					catch (Exception ex)
					{
						MessageBox.Show($"An unexpected error occurred: {ex.Message}. Please contact support.");
					}
					break;

				case "UPDATE":
					if (!CheckAndMarkChanges(db, MemberToEdit!, TBName.Text, TBSurname.Text, CheckIfHasPatronymic(TBPatronymic.Text),
					  DateTime.ParseExact(MTBBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture), byte.Parse(TBAge.Text), MTBAdress.Text, MTBPhoneNumber.Text,
					  PictureController.ImageToByteConvert(pbPhoto.Image)))
					{
						if (MemberToEdit == null) return;
						db.Entry(MemberToEdit).State = EntityState.Unchanged;
						MessageBox.Show("You didn't change member's fields");
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
						MemberToEdit.Photo = PictureController.ImageToByteConvert(pbPhoto.Image);
						MemberToEdit.Patronymic = CheckIfHasPatronymic(TBPatronymic.Text);

						int result = await db.SaveChangesAsync();

						if (result == 1)
						{
							var closeDialog = MessageBox.Show($"{MemberToEdit.Name} {MemberToEdit.Surname} updated successfully. Close the form?", "Update Successful", MessageBoxButtons.YesNo);
							if (closeDialog == DialogResult.Yes)
							{
								Close();
							}
							else
							{
								// Reset tracking of the member and reload the original data
								db.Entry(MemberToEdit).State = EntityState.Detached;  // Detach the entity from the context
								MemberToEdit = await db.Members.FirstOrDefaultAsync(m => m.IIN == MemberToEdit.IIN); // Reload original data
								if (MemberToEdit == null) { MessageBox.Show("Member from data base is null"); return; } //TODO log
																														// Reset fields with the original data
								FillMemberData(MemberToEdit);
								if (MemberToEdit.Photo == null) return; //TODO message and log
								pbPhoto.Image = PictureController.ConvertByteToImage(MemberToEdit.Photo);
								// Focus on the first field
								TBName.Focus();
							}
						}
						else
						{
							MessageBox.Show("You didn't change any data, change or cancel please");
						}
					}
					catch (DbUpdateException ex)
					{
						MessageBox.Show($"Database update error: {ex.Message}");
						if (MemberToEdit == null) return;
						db.Entry(MemberToEdit).Reload();
					}
					catch (Exception ex)
					{
						MessageBox.Show($"An error occurred: {ex.Message}");
					}
					break;
			}
		}

		/// <summary>
		/// Checks if there were any changes in the member's fields and marks the entity as modified if necessary.
		/// </summary>
		/// <param name="db">The database context.</param>
		/// <param name="member">The member to check for changes.</param>
		/// <param name="name">The new name to check.</param>
		/// <param name="surname">The new surname to check.</param>
		/// <param name="patronymic">The new patronymic to check.</param>
		/// <param name="birthDay">The new birth date to check.</param>
		/// <param name="age">The new age to check.</param>
		/// <param name="address">The new address to check.</param>
		/// <param name="phoneNumber">The new phone number to check.</param>
		/// <param name="photo">The new photo in byte array to check.</param>
		/// <returns>True if there were changes, otherwise false.</returns>
		private static bool CheckAndMarkChanges(DbContext db, Member member, string name, string surname, string patronymic, DateTime birthDay, byte age, string address, string phoneNumber, byte[] photo)
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
			if ((member.Photo == null && photo != null) || (member.Photo != null && !member.Photo.SequenceEqual(photo!))) // photo==null can't be cause parameter type byte[], not byte[]?
			{
				db.Entry(member).State = EntityState.Modified;
				hasChanged = true;
			}

			return hasChanged;
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
