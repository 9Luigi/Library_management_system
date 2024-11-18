

namespace Library.Controllers
{
	static internal class ProgressBarController
	{
		static internal async Task pbProgressCgange(Form invokeBy, ProgressBar pb, int startValue, int finalValue)
		{

			//Use invoke cause most likely Form and Progress bar will be in different threads
			await Task.Run(() =>
			{

				pb.Visible = true;
				for (int i = startValue; i < finalValue; i++)
				{
					Task.Delay(100);
					invokeBy.Invoke(new Action(() =>
					{
						pb.PerformStep();
					}));
				}
			});
		}
		static internal async Task pbProgressReset(ProgressBar pb) //await is used than where method called from UI Thread
		{
			await Task.Run(() =>
			{
				pb.Invoke(new Action(() =>
			{
				Task.Delay(100);
				if (pb.Value == 100)
				{
					pb.Value = 0;
					pb.Visible = false;
				}
			}));
			});
		}
	}
}

