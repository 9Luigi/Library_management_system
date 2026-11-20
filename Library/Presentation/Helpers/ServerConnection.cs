using Library.Presentation.Settings;
using Library.Services;
using Microsoft.Extensions.Logging;

namespace Library.Presentation.Controllers
{
	/// <summary>
	/// Every call to the server goes through here. If the server can't be reached, the user chooses:
	/// try again (the same call is repeated) or close the program.
	/// </summary>
	internal static class ServerConnection
	{
		private static readonly ILogger Logger = LoggerService.CreateLogger<FormMain>();

		internal static async Task<T> CallAsync<T>(Func<Task<T>> call)
		{
			while (true)
			{
				try
				{
					return await call();
				}
				// HttpRequestException: no connection; TaskCanceledException: the server didn't answer in time
				catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
				{
					Logger.LogWarning(ex, "Server {Url} is not available", UserSettings.Current.ServerUrl);
					if (!AskRetry())
					{
						// Exits right away: there is nothing the program can do without the server
						Environment.Exit(0);
					}
				}
			}
		}

		internal static Task CallAsync(Func<Task> call) => CallAsync(async () =>
		{
			await call();
			return true;
		});

		private static bool AskRetry()
		{
			var result = MessageBox.Show(
				$"Cannot connect to the server {UserSettings.Current.ServerUrl}.\n" +
				"Make sure Library.Server is running.\n\n" +
				"Retry: try again\nCancel: close the program",
				"Server is not available", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);

			return result == DialogResult.Retry;
		}
	}
}
