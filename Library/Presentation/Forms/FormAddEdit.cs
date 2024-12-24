using Library.Application.Services.CRUD;
using Library.Domain.Models;
using Library.Infrastructure.Repositories;
using Library.Presentation.Controllers;
using Library.Presentation.Controllers.PictureController;
using Library.Properties;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static Library.FormMembers;

namespace Library
{
	//TODO logs via ILogger to XML
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
		private readonly MemberService _memberService;
		/// <summary>
		/// Constructor that initializes the form and subscribes to the MemberCreateOrUpdateEvent event.
		/// Sets the default photo image.
		/// </summary>
		public FaddEdit_prop()
		{
			MemberCreateOrUpdateEvent += ActionRequested; // Subscribe to event, event is invoked on update/create calls
			InitializeComponent();
			pbPhoto.Image = Properties.Resources.NoImage;

			var _dbContext = new LibraryContextForEFcore();
			var _logger = LoggerService.CreateLogger<FaddEdit_prop>();
			_memberService = new(_logger, new GenericRepository<Member>());
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
		/// Handles the event when a create or update action is requested.
		/// Based on the action, it either prepares the form for editing or creating a new member.
		/// </summary>
		internal async void ActionRequested(MemberEventArgs e)
		{
			switch (e.Action)
			{
				case "EDIT":
					BUpdateMember.Visible = true;
					BAddMember.Visible = false;
					MTBIIN.Enabled = false;
					try
					{
						MemberToEdit = await _memberService.GetMemberByIINForUpdateAsync(e.IIN);
						if (MemberToEdit != null)
						{
							FillMemberData(MemberToEdit);
						}
						else
						{
							MessageBoxController.ShowWarning("Cannot load data, probably member was deleted by another employee while you edit, try again please");
						}
					}
					catch (Exception ex)
					{
						MessageBoxController.ShowError($"An error occurred while loading member data: {ex.Message}");
					}
					break;
				case "CREATE":
					MTBIIN.Enabled = true;
					BUpdateMember.Visible = false;
					BAddMember.Visible = true;
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
			bool allFieldsValid = this.Controls
				.OfType<TextBoxBase>()
				.All(textBox => TextBoxBaseController.CheckTextBoxBaseTextOnNull(textBox));

			if (!allFieldsValid) return false;

			bool areRegexValid = new[]
			{
				RegexController.Check(TBName.Text, TBName),
				RegexController.Check(TBSurname.Text, TBSurname),
				RegexController.Check(MTBBirthday.Text, MTBBirthday),
				RegexController.Check(MTBAdress.Text, MTBAdress),
				RegexController.Check(MTBPhoneNumber.Text, MTBPhoneNumber)
			}.All(valid => valid);

			if (!areRegexValid) return false;

			return string.IsNullOrEmpty(TBPatronymic.Text) || TBPatronymic.Text == "None" || RegexController.Check(TBPatronymic.Text, TBPatronymic);
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
					//if(_memberService.IsIINExistsAsync()) //TODO check if IIN exists via MemberService
					Member createdMember = new
						(
							TBName.Text,
							TBSurname.Text,
							DateTime.ParseExact(MTBBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture),
							MTBAdress.Text,
							Convert.ToInt64(MTBIIN.Text),
							MTBPhoneNumber.Text,
							PhotoAsBytes!,
							CheckIfHasPatronymic(TBPatronymic.Text)
						);
					try
					{
						if (await _memberService.CreateMemberAsync(createdMember))
						{
							var result = MessageBoxController.ShowConfirmation("Do you want to add another one?",
									$"{createdMember.Name} {createdMember.Surname} added successfully");

							if (result)
							{
								ResetForm();
							}
							else
							{
								this.Close();
							}
						}
						else MessageBoxController.ShowWarning($"Cannot add {createdMember.Name} {createdMember.Surname} try again later. Inner exception: {{ex.InnerException?.Message ?? \"None\"}}\"");
					}
					catch (DbUpdateException ex)
					{
						ErrorController.HandleException(ex, $"Error updating database: {ex.Message}. Inner exception: {ex.InnerException?.Message ?? "None"}");
					}
					catch (Exception ex)
					{
						ErrorController.HandleException(ex, $"An unexpected error occurred: {ex.Message}. Please contact support. Inner exception: {{ex.InnerException?.Message ?? \"None\"}}\"");
					}
					break;

				case "UPDATE":
					// Directly update the object's properties
					MemberToEdit!.Name = TBName.Text;
					MemberToEdit.Surname = TBSurname.Text;
					MemberToEdit.Patronymic = CheckIfHasPatronymic(TBPatronymic.Text);
					MemberToEdit.BirthDay = DateTime.ParseExact(MTBBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
					MemberToEdit.Age = byte.Parse(TBAge.Text);
					MemberToEdit.Adress = MTBAdress.Text;
					MemberToEdit.PhoneNumber = MTBPhoneNumber.Text;
					MemberToEdit.Photo = PictureController.ImageToByteConvert(pbPhoto.Image);

					// Save the changes to the database
					bool isUpdated = await _memberService.UpdateMemberChangedFieldsAsync(MemberToEdit);

					if (!isUpdated)
					{
						MessageBoxController.ShowWarning("You didn't change member's fields");
						return;
					}
					else
					{
						MessageBoxController.ShowSuccess("Member's fields updated successfully");
					}

					var closeDialog = MessageBoxController.ShowConfirmation($"{MemberToEdit.Name} {MemberToEdit.Surname} updated successfully. Close the form?", "Update Successful");
					if (closeDialog)
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

		private void FaddEdit_prop_FormClosing(object sender, FormClosingEventArgs e)
		{
			_memberService.CloseUpdateContext(); //Dispose update context wich used to update member fields
		}
	}
}
