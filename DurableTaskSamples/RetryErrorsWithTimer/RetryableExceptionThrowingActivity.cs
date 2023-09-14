
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTask.Core.Common;
    using System;
    using System.Threading.Tasks;

    public class RetryableExceptionThrowingActivity : TaskActivity<int, bool>
    {
        private readonly int numThrows;
        private readonly int retryAfterSeconds;
        public RetryableExceptionThrowingActivity(int numThrows = 5, int retryAfterSeconds = 10)
        {
            this.numThrows = numThrows;
            this.retryAfterSeconds = retryAfterSeconds;
        }
        private const string Source = "RetryableExceptionThrowingActivity";
        protected override bool Execute(TaskContext context, int input)
        {
            Logger.Log(Source, "Starting");
            Logger.Log(Source, $"Executing {input}");

            if (input < this.numThrows)
            {
                Logger.Log(Source, "Throwing");
                throw new RetryableWithDelayException(this.retryAfterSeconds, $"My job is to throw {this.numThrows} times.");
            }
            else
            {
                Logger.Log(Source, "Completed");
                return true;
            }

        }
    }
}
