using Library.Application.Services.CRUD;
using Library.Domain.Models;
using Library.Infrastructure.Repositories;
using Library.Presentation.Controllers;
using Library.Presentation.Controllers.PictureController;
using Library.Properties;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Globalization;

namespace Library
{
	public partial class FaddEdit_prop : Form, IDisposable
	{
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string ActionMode { get; set; } = "CREATE"; // Default action mode is CREATE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public long MemberIIN { get; set; }
		byte[]? PhotoAsBytes { get; set; }

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		internal Member? MemberToEdit { get; set; }
		private readonly MemberService _memberService;
		private LibraryContextForEFcore _context; //Disposed in desighner file
		private readonly ControlsController controlsController = new();

		public FaddEdit_prop()
		{
			InitializeComponent();
			pbPhoto.Image = Properties.Resources.NoImage;

			var _logger = LoggerService.CreateLogger<FaddEdit_prop>();
			_context = new LibraryContextForEFcore();
			_memberService = new(_logger, new GenericRepository<Member>(), _context);
		}

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

		private async void BAddMember_Click(object sender, EventArgs e)
		{
			if (CheckFieldsBeforeAction())
			{
				await ActionWithMember("CREATE");
			}
		}

		internal async void ActionRequested()
		{
			switch (ActionMode.ToUpper())
			{
				case "UPDATE":
					BUpdateMember.Visible = true;
					BAddMember.Visible = false;
					MTBIIN.Enabled = false;
					try
					{
						MemberToEdit = await _memberService.GetMemberByIINForUpdateAsync(_context, MemberIIN);
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
						if (await _memberService.CreateMemberAsync(_context, createdMember))
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
						else MessageBoxController.ShowWarning($"Cannot add {createdMember.Name} {createdMember.Surname} try again later.");
					}
					catch (DbUpdateException ex)
					{
						ErrorController.HandleException(ex, $"Error updating database: {ex.Message}.");
					}
					catch (Exception ex)
					{
						ErrorController.HandleException(ex, $"An unexpected error occurred: {ex.Message}.");
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
					bool isUpdated = await _memberService.UpdateMemberChangedFieldsAsync(_context, MemberToEdit);

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
						if (MemberToEdit.Photo == null) return;
						pbPhoto.Image = PictureController.ConvertByteToImage(MemberToEdit.Photo);
						// Focus on the first field
						TBName.Focus();
					}
					break;
			}
		}

		private async void BUpdateMember_Click(object sender, EventArgs e)
		{
			if (CheckFieldsBeforeAction())
			{
				await ActionWithMember("UPDATE");
			}
		}

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

		private void ResetForm()
		{
			TextBoxBaseController.AllTextBoxBaseOnFormClear(this);
			pictureBoxController.pictureBoxImageSetDefault(pbPhoto);
			TBName.Focus();
		}

		private void FaddEdit_prop_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Dispose();
		}

		private void FaddEdit_prop_Load(object sender, EventArgs e)
		{
			ActionRequested();
		}
	}
}