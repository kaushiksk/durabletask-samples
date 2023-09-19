
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTaskSamples.Common.Logging;
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

                Logger.LogVerbose(Source, $"Scheduling FirstActivity");
                bool result = await context.ScheduleTask<bool>(typeof(FirstActivity), input);
                Logger.LogVerbose(Source, $"FirstActivity returned {result}");

                Logger.LogVerbose(Source, $"Scheduling SecondActivity");
                result = await context.ScheduleTask<bool>(typeof(SecondActivity), input + 1);
                Logger.LogVerbose(Source, $"SecondActivity returned {result}");

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
