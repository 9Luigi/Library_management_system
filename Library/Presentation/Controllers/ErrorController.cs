using Microsoft.Extensions.Logging;

namespace Library.Presentation.Controllers
{
	using System;
	using System.Windows.Forms;

	public static class ErrorController
	{
		private static ILogger _logger;

		// Initialize the logger
		public static void Initialize(ILogger logger)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		// Generic method to handle exceptions
		public static void HandleException(Exception ex, string userMessage = "An error occurred.")
		{
			// Log the error
			_logger?.LogError(ex, "Error: {Message}", ex.Message);

			// Show a message box to the user
			MessageBoxController.ShowError(userMessage);
		}

		// Set up global error handling
		public static void SetupGlobalErrorHandling()
		{
			// Handle UI thread exceptions
			Application.ThreadException += (sender, e) =>
			{
				HandleException(e.Exception, "An unexpected error occurred in the application. Contact sys admin");
			};

			// Handle non-UI thread exceptions
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				if (e.ExceptionObject is Exception ex)
				{
					HandleException(ex, "A critical error occurred in the application.");
				}
			};
		}
	}

}
