using Library.Api;
using Library.Contracts.Members;
using Library.Presentation.Controllers;
using Library.Presentation.Theme;
using Library.Presentation.Validation;
using System.ComponentModel;
using System.Globalization;

namespace Library
{
    public partial class FaddEdit_prop : Form, IDisposable
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ActionMode { get; set; } = "CREATE"; // Default action mode is CREATE
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MemberIIN { get; set; } = "";

        public FaddEdit_prop()
        {
            InitializeComponent();
            BCancel.Tag = ButtonRole.Secondary;
            AppTheme.Apply(this);
            lblPersonal.Font = AppTheme.HeaderFont;
            lblContacts.Font = AppTheme.HeaderFont;
        }

        private void BCancel_Click(object sender, EventArgs e) => Close();

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
                    Text = "Edit member";
                    BUpdateMember.Visible = true;
                    BAddMember.Visible = false;
                    MTBIIN.Enabled = false;
                    try
                    {
                        FillMemberData(await ServerConnection.CallAsync(() => MembersApi.GetAsync(MemberIIN)));
                    }
                    catch (ApiException ex)
                    {
                        ErrorHandler.HandleException(ex, ex.Message);
                        Close();
                    }
                    break;
                case "CREATE":
                    Text = "New member";
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
            // 1. Пустые поля
            // Age is calculated by the server, it is not typed in; patronymic is optional (checked below)
            bool allFieldsFilled = this.Controls
                .OfType<TextBoxBase>()
                .Where(tb => tb != TBAge && tb != TBPatronymic)
                .All(TextBoxHelper.CheckTextBoxBaseTextOnNull);

            if (!allFieldsFilled) return false;

            // 2. Обязательные поля по паттернам
            var checks = new (FieldKind Kind, string Value)[]
            {
                (FieldKind.Name,        TBName.Text),
                (FieldKind.Surname,     TBSurname.Text),
                (FieldKind.Birthday,    MTBBirthday.Text),
                (FieldKind.Address,     MTBAdress.Text),
                (FieldKind.PhoneNumber, MTBPhoneNumber.Text),
            };

            foreach (var (kind, value) in checks)
            {
                var result = InputValidator.Validate(kind, value);
                if (!result.IsValid)
                {
                    MessageBoxHelper.ShowWarning(result.Error!);
                    return false;
                }
            }

            // 3. Отчество — необязательное
            bool patronymicOmitted =
                string.IsNullOrEmpty(TBPatronymic.Text) || TBPatronymic.Text == "None";

            if (!patronymicOmitted)
            {
                var result = InputValidator.Validate(FieldKind.Patronymic, TBPatronymic.Text);
                if (!result.IsValid)
                {
                    MessageBoxHelper.ShowWarning(result.Error!);
                    return false;
                }
            }

            return true;
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
            var birthDay = DateTime.ParseExact(MTBBirthday.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            try
            {
                switch (operation)
                {
                    case "CREATE":
                        var createRequest = new CreateMemberRequest(
                            MTBIIN.Text,
                            TBName.Text,
                            TBSurname.Text,
                            CheckIfHasPatronymic(TBPatronymic.Text),
                            birthDay,
                            MTBAdress.Text,
                            MTBPhoneNumber.Text);

                        var created = await ServerConnection.CallAsync(() => MembersApi.CreateAsync(createRequest));

                        if (MessageBoxHelper.ShowConfirmation("Do you want to add another one?",
                                $"{created.Name} {created.Surname} added successfully"))
                        {
                            ResetForm();
                        }
                        else
                        {
                            Close();
                        }
                        break;

                    case "UPDATE":
                        var updateRequest = new UpdateMemberRequest(
                            TBName.Text,
                            TBSurname.Text,
                            CheckIfHasPatronymic(TBPatronymic.Text),
                            birthDay,
                            MTBAdress.Text,
                            MTBPhoneNumber.Text);

                        var updated = await ServerConnection.CallAsync(() => MembersApi.UpdateAsync(MemberIIN, updateRequest));

                        if (MessageBoxHelper.ShowConfirmation($"{updated.Name} {updated.Surname} updated successfully. Close the form?", "Update Successful"))
                        {
                            Close();
                        }
                        else
                        {
                            // The server recalculates the age, show what it saved
                            FillMemberData(updated);
                            TBSurname.Focus();
                        }
                        break;
                }
            }
            catch (ApiException ex)
            {
                ErrorHandler.HandleException(ex, ex.Message);
            }
        }

        private async void BUpdateMember_Click(object sender, EventArgs e)
        {
            if (CheckFieldsBeforeAction())
            {
                await ActionWithMember("UPDATE");
            }
        }

        private void FillMemberData(MemberDto member)
        {
            MTBIIN.Text = member.IIN;
            TBName.Text = member.Name;
            TBSurname.Text = member.Surname;
            TBPatronymic.Text = member.Patronymic;
            MTBBirthday.Text = member.BirthDay.ToString("dd/MM/yyyy");
            TBAge.Text = member.Age.ToString();
            MTBAdress.Text = member.Address;
            MTBPhoneNumber.Text = member.PhoneNumber;
        }

        private void ResetForm()
        {
            TextBoxHelper.AllTextBoxBaseOnFormClear(this);
            TBSurname.Focus();
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