using DurableTask.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DurableTaskSamples
{
    public static class Utils
    {
        /// <summary>
        /// ErrorPropagationMode.SerializeExceptions does not work with Azure.Storage for some reason
        /// Using this workaround to simulate custom exception handling
        /// </summary>
        /// <param name="ex">TaskFailedException</param>
        /// <returns>true if this is custom exception</returns>
        public static bool IsCustomRetryException(TaskFailedException ex)
        {
            return !string.IsNullOrEmpty(ex.Message) && ex.Message.Contains(RetryableWithDelayException.IdentifierString);
        }

        /// <summary>
        /// ErrorPropagationMode.SerializeExceptions does not work with Azure.Storage for some reason
        /// Using this workaround to simulate custom exception handling.
        /// Ideally we would be getting this directly from the deserialized RetryableWithDelayException
        /// </summary>
        /// <param name="ex">TaskFailedException</param>
        /// <returns>retry after value if present else 1</returns>
        public static int GetRetryAfterSecondsFromException(TaskFailedException ex)
        {
            var retryAfterStr = ex.Message.Split(new string[] { RetryableWithDelayException.IdentifierString }, StringSplitOptions.RemoveEmptyEntries)[1];
            if (int.TryParse(retryAfterStr, out int retryAfter))
            {
                return retryAfter;
            }
            else
            {
                return 1;
            }
        }
    }
}
