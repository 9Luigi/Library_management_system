using Library.Controllers.PictureController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Library.Controllers
{
    internal static class TextBoxBaseController
    {
        static internal void AllTextBoxBaseOnFormClear(Form form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is TextBoxBase tbb)
                {
                    tbb.Text = null;
                }
            }
        }
        static internal bool CheckTextBoxBaseTextOnNull(TextBoxBase textBoxBase)
        {//check properties for null and by RegexController

            if (string.IsNullOrWhiteSpace(textBoxBase.Text))
            {
                MessageBox.Show(textBoxBase.Name + " is empty, fill in the empty requiered(*) fields");
                return false;
            }
            if (textBoxBase.Text.Length > 75)
            {
                MessageBox.Show($"{textBoxBase.Name} cannot be more than 75 symbols");
                return false;
            }
            else return true;
		}
		//TODO to adopt
		/*//{private bool UpdateField<T>(ref T field, T newValue)
{
    if (!EqualityComparer<T>.Default.Equals(field, newValue))
    {
        field = newValue;
        return true;
    }
    return false;
}

try
{
	bool hasChanges = false;

	// Применяем обновления только если есть изменения
	hasChanges |= UpdateField(ref MemberToEdit.Name, TBName.Text);
	hasChanges |= UpdateField(ref MemberToEdit.Surname, TBSurname.Text);
	hasChanges |= UpdateField(ref MemberToEdit.BirthDay, DateTime.Parse(MTBBirthday.Text));
	hasChanges |= UpdateField(ref MemberToEdit.Age, byte.Parse(TBAge.Text));
	hasChanges |= UpdateField(ref MemberToEdit.Adress, MTBAdress.Text);
	hasChanges |= UpdateField(ref MemberToEdit.PhoneNumber, MTBPhoneNumber.Text);

	var newPhoto = pbPhoto.Image != null ? PictureController.ImageToByteConvert(pbPhoto.Image!) : null;
	hasChanges |= UpdateField(ref MemberToEdit.Photo, newPhoto);

	hasChanges |= UpdateField(ref MemberToEdit.Patronymic, CheckIfHasPatronymic(TBPatronymic.Text));

	// Если нет изменений, выходим
	if (!hasChanges)
	{
		MessageBox.Show("You didn't change member's fields");
		return;
	}
}*/
	}
}
