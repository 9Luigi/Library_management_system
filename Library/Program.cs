using Library.Presentation.Controllers;
using Serilog;

namespace Library
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			// Initialize the logger
			var logger = LoggerService.CreateLogger<FormMain>(); // Replace with your actual logger implementation
			ErrorController.Initialize(logger);

			// Set up global error handling
			ErrorController.SetupGlobalErrorHandling();

			// To customize application configuration such as set high DPI settings or default font,
			// see https://aka.ms/applicationconfiguration.
			ApplicationConfiguration.Initialize();
			System.Windows.Forms.Application.Run(new FormMain());
		}
    }
}