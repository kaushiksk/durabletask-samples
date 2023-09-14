
namespace DtfTester
{
    using DurableTask.Core;
    using DurableTask.Core.Exceptions;
    using System;
    using System.Threading.Tasks;

    public class ErrorHandlingWithContinueAsNewOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "ErrorHandlingWithTimerOrchestration";
        public override async Task<bool> RunTask(OrchestrationContext context, int input)
        {
            Logger.Log(Source, "Starting");
            var retryOptions = new RetryOptions(TimeSpan.FromSeconds(1), 2)
            {
                Handle = (ex) =>
                {
                    Logger.Log(Source, ex.GetType().Name);
                    return !Utils.IsCustomRetryException((TaskFailedException)ex);
                }
            };
            
            try
            {
                bool result = await context.ScheduleWithRetry<bool>(typeof(RetryableExceptionThrowingActivity), retryOptions, input);
                Logger.Log(Source, $"RetryableExceptionThrowingActivity returned {result}");
                Logger.Log(Source, "Completed");
                return result;
            }
            catch (TaskFailedException ex)
            {
                int retryAfterInSeconds = Utils.GetRetryAfterSecondsFromException(ex);
                Logger.Log(Source, $"Error in activity, scheduling retry after {retryAfterInSeconds}");
                int newInput = await context.CreateTimer<int>(context.CurrentUtcDateTime.AddSeconds(retryAfterInSeconds), input + 1);
                Logger.Log(Source, "Timer elapsed");
                context.ContinueAsNew(newInput);
            }
            
            
            return false;
        }
    }
}
