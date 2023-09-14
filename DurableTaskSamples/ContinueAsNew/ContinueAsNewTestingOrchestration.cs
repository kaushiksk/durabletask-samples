namespace DurableTaskSamples
{
    using DurableTask.Core;
    using System;
    using System.Threading.Tasks;

    public class ContinueAsNewTestingOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "ContinueAsNewTestingOrchestration";

        public override async Task<bool> RunTask(OrchestrationContext context, int input)
        {
            try
            {
                Logger.Log(Source, $"Initiating, IsReplaying: {context.IsReplaying}");
                await context.ScheduleTask<bool>(typeof(GreetingActivity), input);
                
                if (input < 3)
                {
                    context.ContinueAsNew(input + 1);
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
