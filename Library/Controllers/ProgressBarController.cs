
namespace Library.Controllers
{
	/// <summary>
	/// Controller for managing ProgressBar operations.
	/// </summary>
	static internal class ProgressBarController
	{
	
		//TODO to LOG
		/// <summary>
		/// Updates the ProgressBar from the starting value to the final value.
		/// </summary>
		/// <param name="invokeBy">The form invoking the method (used for invoking UI updates).</param>
		/// <param name="pb">The ProgressBar to update.</param>
		/// <param name="startValue">The starting value of the ProgressBar.</param>
		/// <param name="finalValue">The final value of the ProgressBar.</param>
		static internal async Task pbProgressChange(Form invokeBy, ProgressBar pb, int startValue, int finalValue)
		{
			try
			{
				// Run in a background thread
				await Task.Run(async () =>
				{
					invokeBy.Invoke(new Action(() => { pb.Visible = true; }));

					for (int i = startValue; i <= finalValue; i++)
					{
						await Task.Delay(1); // Delay for better user experience
						invokeBy.Invoke(new Action(() =>
						{
							pb.PerformStep();
						}));
					}
				});
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		/// <summary>
		/// Resets the ProgressBar to its initial state.
		/// </summary>
		/// <param name="pb">The ProgressBar to reset.</param>
		static internal async Task pbProgressReset(ProgressBar pb)
		{
			try
			{
				await Task.Run(async () =>
				{
					await Task.Delay(500); // Delay for better user experience
					pb.Invoke(new Action(() =>
					{
						if (pb.Value == 100)
						{
							pb.Value = 0;
							pb.Visible = false;
						}
					}));
				});
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
