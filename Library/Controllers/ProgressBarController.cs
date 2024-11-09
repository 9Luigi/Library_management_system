

namespace Library.Controllers
{
    static internal class ProgressBarController
    {
        static internal async Task pbProgressCgange(Form invokeBy, ProgressBar pb, int startValue, int finalValue)
        {
            
            //Use invoke cause most likely Form and Progress bar will be in different threads
            invokeBy.Invoke(new Action(() =>
            {
				pb.Visible = true;
				for (int i = startValue; i < finalValue; i++)
                {
                    pb.PerformStep();
                    Task.Delay(100);
                }
            }));
        }
        static internal async Task pbProgressReset(ProgressBar pb) //await is used than where method called from UI Thread
        {
            pb.Invoke(new Action(() =>
            {
                if (pb.Value == 100)
                {
                    pb.Value = 0;
                    pb.Visible = false;
                }
            }));
        }
    }
}

