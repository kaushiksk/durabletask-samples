
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using System;
    using System.Threading.Tasks;

    public class MultipleActivitiesTestingOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "MultipleActivitiesTestingOrchestration";

        public override async Task<bool> RunTask(OrchestrationContext context, int input)
        {
            try
            {
                Logger.Log(Source, $"Initiating, IsReplaying: {context.IsReplaying}");
                bool result = await context.ScheduleTask<bool>(typeof(FirstActivity), input);
                Logger.Log(Source, $"FirstActivity returned {result}");

                result = await context.ScheduleTask<bool>(typeof(SecondActivity), input + 1);
                Logger.Log(Source, $"SecondActivity returned {result}");
                Logger.Log(Source, "Completed");
                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(Source, ex.ToString());
                return false;
            }
        }
    }
}
