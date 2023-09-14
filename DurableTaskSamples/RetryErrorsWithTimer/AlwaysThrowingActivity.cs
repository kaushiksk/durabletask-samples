
namespace DurableTaskSamples
{
    using DurableTask.Core;
    using DurableTask.Core.Common;
    using System;
    using System.Threading.Tasks;

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
