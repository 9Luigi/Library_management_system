
namespace Library.Presentation.Controllers
{
    internal static class TextBoxHelper
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
                MessageBoxHelper.ShowWarning(textBoxBase.Name + " is empty, fill in the empty requiered(*) fields");
                return false;
            }
            if (textBoxBase.Text.Length > 75)
            {
                MessageBoxHelper.ShowWarning($"{textBoxBase.Name} cannot be more than 75 symbols");
                return false;
            }
            else return true;
        }
    }
}
