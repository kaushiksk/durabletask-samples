
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTaskSamples.Common.Exceptions;
    using DurableTaskSamples.Common.Logging;

    public class AlwaysThrowingActivity : TaskActivity<int, bool>
    {
        private readonly int retryAfterSeconds;
        
        public AlwaysThrowingActivity(int retryAfterSeconds = 5)
        {
            this.retryAfterSeconds = retryAfterSeconds;
        }

        private const string Source = "AlwaysThrowingActivity";

        protected override bool Execute(TaskContext context, int input)
        {
            Logger.Log(Source, "Starting");
            Logger.Log(Source, $"Executing {input}");

            Logger.Log(Source, "Throwing");
            throw new RetryableWithDelayException(this.retryAfterSeconds, $"My job is to throw always. ");
        }
    }
}
