using Microsoft.Extensions.Logging;
using System.Windows.Forms;

namespace Library.Presentation.Controllers
{
	/// <summary>
	/// Controller for managing ProgressBar operations.
	/// </summary>
	internal static class ProgressBarController
	{
		private static readonly Lazy<ILogger> _loggerLazy = new(() =>
		{
			return LoggerService.CreateLogger<ILogger>(); 
		});

		private static ILogger _logger => _loggerLazy.Value;

		/// <summary>
		/// Updates the ProgressBar from the starting value to the final value.
		/// </summary>
		/// <param name="invokeBy">The form invoking the method (used for invoking UI updates).</param>
		/// <param name="pb">The ProgressBar to update.</param>
		/// <param name="startValue">The starting value of the ProgressBar.</param>
		/// <param name="finalValue">The final value of the ProgressBar.</param>
		/// <exception cref="ArgumentNullException">Thrown if ProgressBar or invoking form is null.</exception>
		public static async Task UpdateProgressAsync(Form invokeBy, ProgressBar pb, int startValue, int finalValue)
		{
			if (pb == null) throw new ArgumentNullException(nameof(pb), "ProgressBar cannot be null.");
			if (invokeBy == null) throw new ArgumentNullException(nameof(invokeBy), "Invoking form cannot be null.");
			if (startValue < 0 || finalValue > pb.Maximum || startValue > finalValue)
				throw new ArgumentOutOfRangeException(nameof(startValue), "Invalid start or final value for ProgressBar.");

			try
			{
				_logger?.LogInformation("Progress update started. StartValue: {StartValue}, FinalValue: {FinalValue}", startValue, finalValue);

				// Ensure visibility on the UI thread
				invokeBy.Invoke(() => { pb.Visible = true; pb.Value = startValue; });

				for (int i = startValue; i <= finalValue; i++)
				{
					await Task.Delay(10); // Delay to simulate progress
					invokeBy.Invoke(() => { pb.Value = i; });
				}

				_logger?.LogInformation("Progress update completed successfully.");
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "An error occurred while updating ProgressBar.");
				throw;
			}
		}

		/// <summary>
		/// Resets the ProgressBar to its initial state.
		/// </summary>
		/// <param name="pb">The ProgressBar to reset.</param>
		/// <exception cref="ArgumentNullException">Thrown if ProgressBar is null.</exception>
		public static async Task ResetProgressAsync(ProgressBar pb)
		{
			if (pb == null) throw new ArgumentNullException(nameof(pb), "ProgressBar cannot be null.");

			try
			{
				_logger?.LogInformation("ProgressBar reset started.");

				// Simulate a delay for better UX
				await Task.Delay(500);

				// Reset ProgressBar on UI thread
				pb.Invoke(() =>
				{
					if (pb.Value == pb.Maximum)
					{
						pb.Value = 0;
						pb.Visible = false;
					}
				});

				_logger?.LogInformation("ProgressBar reset completed successfully.");
			}
			catch (Exception ex)
			{
				_logger?.LogError(ex, "An error occurred while resetting ProgressBar.");
				throw;
			}
		}
	}
}
