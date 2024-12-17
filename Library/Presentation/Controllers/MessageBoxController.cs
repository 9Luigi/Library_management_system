namespace Library.Presentation.Controllers
{
	public static class MessageBoxController //TODO implement in the application
	{
		public static void ShowError(string message)
		{
			ShowMessage(message, "Error", MessageBoxIcon.Error);
		}

		public static void ShowInfo(string message)
		{
			ShowMessage(message, "Information", MessageBoxIcon.Information);
		}

		public static void ShowWarning(string message)
		{
			ShowMessage(message, "Warning", MessageBoxIcon.Warning);
		}

		public static void ShowSuccess(string message)
		{
			ShowMessage(message, "Success", MessageBoxIcon.Information); 
		}

		private static void ShowMessage(string message, string caption, MessageBoxIcon icon)
		{
			MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
		}
	}

}
