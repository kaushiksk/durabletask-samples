
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTask.Core.Common;
    using DurableTaskSamples.Common.Logging;
    using System;
    using System.Threading.Tasks;

    public class PollingActivity : AsyncTaskActivity<int, bool>
    {
        private readonly int numPolls;

        public PollingActivity(int numPolls = 8)
        {
            this.numPolls = numPolls;
        }

        private const string Source = "PollingActivity";
        protected override async Task<bool> ExecuteAsync(TaskContext context, int input)
        {
            Logger.Log(Source, "Starting");

            Logger.Log(Source, $"Performing async poll task attempt {input}");
            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(true);
            
            bool pollingResult = !(input < numPolls);
            Logger.Log(Source, $"Polling result: {pollingResult}");

            return pollingResult;
        }
    }
}
