
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using System;
    using System.Threading.Tasks;

    public class UnboundedPollingWithInlineRetriesOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "UnboundedPollingWithInlineRetriesOrchestration";
        private const int PollingIntervalInSeconds = 10;
        private const int MaxPollingPerOrchestrationInstance = 10;

        public override async Task<bool> RunTask(OrchestrationContext context, int input)
        {
            Logger.Log(Source, $"Initiating, IsReplaying: {context.IsReplaying}");

            bool result = false;
            for (int i = 0; i < MaxPollingPerOrchestrationInstance; i++)
            {
                Logger.Log(Source, $"Polling attempt {input + i}");
                result = await context.ScheduleTask<bool>(typeof(PollingActivity), input + i);

                if (result)
                {
                    Logger.Log(Source, "Polling success");
                    break;
                }
                else
                {
                    Logger.Log(Source, $"Scheduling next poll after {PollingIntervalInSeconds} seconds.");
                    await context.CreateTimer<int>(context.CurrentUtcDateTime.AddSeconds(PollingIntervalInSeconds), input + 1);
                }
            }

            if (result)
            {
                Logger.Log(Source, "Completed");
                return true;
            }
            else
            {
                Logger.Log(Source, $"{input + MaxPollingPerOrchestrationInstance} retries were not enough, continuing as new");
                context.ContinueAsNew(input + MaxPollingPerOrchestrationInstance);
            }

            return false;
        }
    }
}
