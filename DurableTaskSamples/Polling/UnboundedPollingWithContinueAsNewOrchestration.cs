
namespace DtfTester
{
    using DurableTask.Core;
    using System;
    using System.Threading.Tasks;

    public class UnboundedPollingWithContinueAsNewOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "UnboundedPollingWithContinueAsNewOrchestration";
        private const int PollingIntervalInSeconds = 10;

        public override async Task<bool> RunTask(OrchestrationContext context, int input)
        {
            Logger.Log(Source, $"Initiating, IsReplaying: {context.IsReplaying}");

            Logger.Log(Source, $"Polling attempt {input}");
            bool result = await context.ScheduleTask<bool>(typeof(PollingActivity), input);

            if (result)
            {
                Logger.Log(Source, "Polling success");
                Logger.Log(Source, "Completed");
                return true;
            }
            else
            {
                Logger.Log(Source, $"Polling did not return success, scheduling next poll after {PollingIntervalInSeconds} seconds.");
                int newInput = await context.CreateTimer<int>(context.CurrentUtcDateTime.AddSeconds(PollingIntervalInSeconds), input + 1);
                Logger.Log(Source, $"Timer expired, continuing as new");
                context.ContinueAsNew(newInput);
            }

            return false;
        }
    }
}
