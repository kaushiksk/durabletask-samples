
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Basic orchestration scheduling multiple activities in sequence.
    /// 
    /// We notice the following things here:
    ///   - The orchestration runs multiple times, but each activity only executes once
    ///   - The orchestration completes execution as soon as it finds an activity to schedule
    /// </summary>
    public class MultipleActivitiesOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "MultipleActivitiesOrchestration";

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
