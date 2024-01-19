using DurableTask.AzureStorage;
using DurableTask.Core.Exceptions;
using DurableTaskSamples.Common.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;

namespace DurableTaskSamples.Common.Utils
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

        public static AzureStorageOrchestrationService GetAzureOrchestrationServiceClient()
        {
            var storageConnectionString = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
            if (string.IsNullOrEmpty(storageConnectionString))
            {
                Console.WriteLine("Azure Storage Connection String is empty, please provide valid connection string");
                Environment.Exit(0);
            }

            var taskHubName = ConfigurationManager.AppSettings["TaskHubName"];
            var azureStorageSettings = new AzureStorageOrchestrationServiceSettings
            {
                StorageAccountDetails = new StorageAccountDetails { ConnectionString = storageConnectionString },
                TaskHubName = taskHubName,
            };

            bool shouldLogAzureStorageTraces = bool.Parse(ConfigurationManager.AppSettings["LogAzureStorageTraces"]);
            if (shouldLogAzureStorageTraces)
            {
                azureStorageSettings.LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            }

            var orchestrationServiceAndClient = new AzureStorageOrchestrationService(azureStorageSettings);
            return orchestrationServiceAndClient;
        }

        public static bool ShouldLogDtfCoreTraces()
        {
            return bool.Parse(ConfigurationManager.AppSettings["LogDtfCoreEventTraces"]);
        }

        public static bool ShouldDisableVerboseLogsInOrchestration()
        {
            return bool.Parse(ConfigurationManager.AppSettings["DisableOrchestrationVerboseLogs"]);
        }

        public static bool ShouldLaunchInstanceManager()
        {
            return bool.Parse(ConfigurationManager.AppSettings["LaunchInstanceManager"]);
        }

        public static void WriteToConsoleWithColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
