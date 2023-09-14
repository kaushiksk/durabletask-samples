namespace DurableTaskSamples
{
    using DurableTask.Core;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Basic orchestration while schedules the same activity multiple times
    /// We test the following things here:
    ///   - When the same activity is schedule multiple times separately, each is a separate instance
    ///   - Same activity can be scheduled again with same input - the orchestration is smart enough to
    ///     identify that this is a separate invocation and not the first one.
    ///   - Each instance of the activity is only executed once, even the orchestration runs multiple times
    /// </summary>
    public class SameActivityMultipleSchedulesOrchestration: TaskOrchestration<bool, int>
    {
        private const string Source = "SameActivityMultipleSchedulesOrchestration";

        public override async Task<bool> RunTask(OrchestrationContext context, int input)
        {
            try
            {
                Logger.Log(Source, $"Initiating, IsReplaying: {context.IsReplaying}");
                await context.ScheduleTask<bool>(typeof(GreetingActivity), input);
                await context.ScheduleTask<bool>(typeof(GreetingActivity), input);
                await context.ScheduleTask<bool>(typeof(GreetingActivity), 42);
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
