
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTaskSamples.Common.Logging;
    using System;
    using System.Threading.Tasks;

    public class FixedPollingWithInlineRetriesOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "FixedPollingWithInlineRetriesOrchestration";
        private const int PollingIntervalInSeconds = 10;

        public override async Task<bool> RunTask(OrchestrationContext context, int input)
        {
            Logger.Log(Source, $"Initiating, IsReplaying: {context.IsReplaying}");

            for (int i = 0; i < input; i++)
            {
                Logger.LogVerbose(Source, $"Polling attempt {i}");
                bool result = await context.ScheduleTask<bool>(typeof(PollingActivity), i);

                if (result)
                {
                    Logger.Log(Source, "Polling success");
                    break;
                }
                else
                {
                    Logger.LogVerbose(Source, $"Scheduling next poll after {PollingIntervalInSeconds} seconds.");
                    await context.CreateTimer<int>(context.CurrentUtcDateTime.AddSeconds(PollingIntervalInSeconds), input + 1);
                    Logger.LogVerbose(Source, $"Poll timer for attempt {i} elapsed.");
                }
            }

            Logger.Log(Source, "Completed");
            return true;
        }
    }
}
