
namespace DtfTester
{
    using DurableTask.Core;
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
