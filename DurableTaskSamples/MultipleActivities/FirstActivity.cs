
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTaskSamples.Common.Logging;
    using System.Threading.Tasks;

    public class FirstActivity : TaskActivity<int, bool>
    {
        private const string Source = "FirstActivity";
        protected override bool Execute(TaskContext context, int input)
        {
            Logger.Log(Source, "Starting");
            Logger.Log(Source, $"Executing {input}");
            Logger.Log(Source, "Completed");
            return true;
        }
    }
}
