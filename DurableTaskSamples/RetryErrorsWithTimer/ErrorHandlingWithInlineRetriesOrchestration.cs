
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTask.Core.Exceptions;
    using DurableTaskSamples.Common.Logging;
    using DurableTaskSamples.Common.Utils;
    using System;
    using System.Threading.Tasks;

    public class ErrorHandlingWithInlineRetriesOrchestration : TaskOrchestration<bool, int>
    {
        private const string Source = "ErrorHandlingWithInlineRetriesOrchestration";
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

            for (int i = 0; i < input; i++)
            {
                Logger.Log(Source, $"Attempt {i}");
                try
                {
                    bool result = await context.ScheduleWithRetry<bool>(typeof(AlwaysThrowingActivity), retryOptions, i);
                    Logger.Log(Source, $"AlwaysThrowingActivity returned {result}");
                    return result;
                }
                catch (TaskFailedException ex)
                {
                    int retryAfterInSeconds = Utils.GetRetryAfterSecondsFromException(ex);
                    Logger.Log(Source, $"Error in activity, scheduling retry after {retryAfterInSeconds}");
                    int newInput = await context.CreateTimer<int>(context.CurrentUtcDateTime.AddSeconds(retryAfterInSeconds), input + 1);
                    Logger.Log(Source, "Timer elapsed");
                }
            }

            Logger.Log(Source, "Retry attempts exhausted");
            return false;
        }
    }
}
