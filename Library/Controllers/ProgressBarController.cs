

namespace Library.Controllers
{
    static internal class ProgressBarController
    {
        static internal void pbProgressCgange(Form invokeBy, ProgressBar pb, int startValue, int finalValue)
        {
            pb.Visible = true;
            //Use invoke cause most likely Form and Progress bar will be in different threads
            invokeBy.Invoke(new Action(() =>
            {
                for (int i = startValue; i < finalValue; i++)
                {
                    pb.PerformStep();
                    Task.Delay(100);
                }
            }));
        }
        static internal void pbProgressReset(ProgressBar pb)
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
