using System;
using System.Runtime.Serialization;

namespace DurableTaskSamples.Common.Exceptions
{
    public class RetryableWithDelayException : Exception, ISerializable
    {
        public static readonly string IdentifierString = "Expected to retry after ";

        public RetryableWithDelayException(int retryAfter, string message)
            : base(message + IdentifierString + retryAfter.ToString())
        {
            RetryAfterInSeconds = retryAfter;
        }

        protected RetryableWithDelayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            RetryAfterInSeconds = info.GetInt32(nameof(RetryAfterInSeconds));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(RetryAfterInSeconds), RetryAfterInSeconds);
        }

        public int RetryAfterInSeconds { get; set; }
    }
}
