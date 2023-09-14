namespace DtfTester
{
    using DurableTask.Core;
    using System;
    using System.Threading.Tasks;

    public class DtfTestingOrchestration: TaskOrchestration<bool, int>
    {
        private const string Source = "DtfTestingOrchestration";

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
