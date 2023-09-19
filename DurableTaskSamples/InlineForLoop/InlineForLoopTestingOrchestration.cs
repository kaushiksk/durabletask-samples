
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using System;
    using System.Threading.Tasks;

    public class InlineForLoopTestingOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "InlineForLoopTestingOrchestration";

        public override async Task<bool> RunTask(OrchestrationContext context, int input)
        {
            try
            {
                Logger.Log(Source, $"Initiating, IsReplaying: {context.IsReplaying}");

                for (int i = 0; i < input; i++)
                {
                    Logger.Log(Source, $"Executing for {i}");
                    await context.ScheduleTask<bool>(typeof(GreetingActivity), i);
                }

                Logger.Log(Source, "Completed");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(Source, ex.ToString());
                return false;
            }
        }
    }
}
