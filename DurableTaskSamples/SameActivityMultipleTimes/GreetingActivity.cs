
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTaskSamples.Common.Logging;
    using System.Threading.Tasks;

    public class GreetingActivity: AsyncTaskActivity<int, bool>
    {
        private const string Source = "GreetingActivity";
        protected override async Task<bool> ExecuteAsync(TaskContext context, int input)
        {
            Logger.Log(Source, "Starting");
            await Task.Delay(5).ConfigureAwait(false);
            Logger.Log(Source, $"Executing {input}");
            Logger.Log(Source, "Completed");

            await Task.Delay(2000);
            
            if (input < 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
