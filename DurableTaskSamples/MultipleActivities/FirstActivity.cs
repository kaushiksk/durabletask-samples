
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using System.Threading.Tasks;

    public class FirstActivity : TaskActivity<int, bool>
    {
        private const string Source = "FirstActivity";
        protected override bool Execute(TaskContext context, int input)
        {
            Logger.Log(Source, "Starting");
            // await Task.Delay(5).ConfigureAwait(false);
            Logger.Log(Source, $"Executing {input}");
            Logger.Log(Source, "Completed");
            return true;
        }
    }
}
