﻿namespace DtfTester
{
    using DurableTask.AzureStorage;
    using DurableTask.Core;
    using DurableTask.Core.Tracing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Program
    {
        private static async Task<OrchestrationInstance> StartDtfTestingOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(DtfTestingOrchestration), instanceId, 1);
            return instance;
        }

        private static async Task<OrchestrationInstance> StartMultipleActivitiesTestingOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(MultipleActivitiesTestingOrchestration), instanceId, 1);
            return instance;
        }

        private static async Task<OrchestrationInstance> StartContinueAsNewTestingOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(ContinueAsNewTestingOrchestration), instanceId, 0);

            return instance;
        }

        private static async Task<OrchestrationInstance> StartErrorHandlingWithContinueAsNewOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(ErrorHandlingWithContinueAsNewOrchestration), instanceId, 0);
            return instance;
        }

        private static async Task<OrchestrationInstance> StartInlineRetriesTestingOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(InlineRetriesTestingOrchestration), instanceId, 5);
            return instance;
        }

        private static async Task<OrchestrationInstance> StartErrorHandlingWithInlineRetriesOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(ErrorHandlingWithInlineRetriesOrchestration), instanceId, 5);
            return instance;
        }

        private static async Task<OrchestrationInstance> StartFixedPollingWithInlineRetriesOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(FixedPollingWithInlineRetriesOrchestration), instanceId, 15);
            return instance;
        }

        private static async Task<OrchestrationInstance> StartUnboundedPollingWithInlineRetriesOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(UnboundedPollingWithInlineRetriesOrchestration), instanceId, 0);
            return instance;
        }

        private static async Task<OrchestrationInstance> StartUnboundedPollingWithContinueAsNewOrchestration(string instanceId, TaskHubClient taskHubClient)
        {
            var instance = await taskHubClient.CreateOrchestrationInstanceAsync(typeof(UnboundedPollingWithContinueAsNewOrchestration), instanceId, 0);
            return instance;
        }

        private static async Task InitializeTaskWorker(TaskHubWorker taskHubWorker)
        {
            taskHubWorker.AddTaskOrchestrations(
                typeof(DtfTestingOrchestration),
                typeof(MultipleActivitiesTestingOrchestration),
                typeof(ContinueAsNewTestingOrchestration),
                typeof(ErrorHandlingWithContinueAsNewOrchestration),
                typeof(InlineRetriesTestingOrchestration),
                typeof(ErrorHandlingWithInlineRetriesOrchestration),
                typeof(FixedPollingWithInlineRetriesOrchestration),
                typeof(UnboundedPollingWithInlineRetriesOrchestration),
                typeof(UnboundedPollingWithContinueAsNewOrchestration));
            taskHubWorker.AddTaskActivities(
                new GreetingActivity(),
                new FirstActivity(),
                new SecondActivity(),
                new RetryableExceptionThrowingActivity(),
                new PollingActivity(),
                new AlwaysThrowingActivity());
            await taskHubWorker.StartAsync();
        }

        private static readonly Dictionary<int, string> commandLineOptions = new Dictionary<int, string>()
        {
            { 1, nameof(DtfTestingOrchestration)},
            { 2, nameof(MultipleActivitiesTestingOrchestration)},
            { 3, nameof(ContinueAsNewTestingOrchestration)},
            { 4, nameof(ErrorHandlingWithContinueAsNewOrchestration)},
            { 5, nameof(InlineRetriesTestingOrchestration)},
            { 6, nameof(ErrorHandlingWithInlineRetriesOrchestration)},
            { 7, nameof(FixedPollingWithInlineRetriesOrchestration)},
            { 8, nameof(UnboundedPollingWithInlineRetriesOrchestration)},
            { 9, nameof(UnboundedPollingWithContinueAsNewOrchestration)},
        };
        private static Dictionary<string, Func<Task<OrchestrationInstance>>> testInstances;

        private static void InitializeTests(string instanceId, TaskHubClient taskHubClient)
        {
            testInstances = new Dictionary<string, Func<Task<OrchestrationInstance>>>()
            {
                { nameof(DtfTestingOrchestration), async () => await StartDtfTestingOrchestration(instanceId, taskHubClient) },
                { nameof(MultipleActivitiesTestingOrchestration), async () => await StartMultipleActivitiesTestingOrchestration(instanceId, taskHubClient) },
                { nameof(ContinueAsNewTestingOrchestration), async() => await StartContinueAsNewTestingOrchestration(instanceId, taskHubClient) },
                { nameof(ErrorHandlingWithContinueAsNewOrchestration), async () => await  StartErrorHandlingWithContinueAsNewOrchestration(instanceId, taskHubClient) },
                { nameof(InlineRetriesTestingOrchestration), async() => await StartInlineRetriesTestingOrchestration(instanceId, taskHubClient) },
                { nameof(ErrorHandlingWithInlineRetriesOrchestration), async() => await StartErrorHandlingWithInlineRetriesOrchestration(instanceId, taskHubClient) },
                { nameof(FixedPollingWithInlineRetriesOrchestration), async() => await StartFixedPollingWithInlineRetriesOrchestration(instanceId, taskHubClient) },
                { nameof(UnboundedPollingWithInlineRetriesOrchestration), async() => await StartUnboundedPollingWithInlineRetriesOrchestration(instanceId, taskHubClient) },
                { nameof(UnboundedPollingWithContinueAsNewOrchestration), async() => await StartUnboundedPollingWithContinueAsNewOrchestration(instanceId, taskHubClient) },
            };
        }

        private static void PrintCommandLine()
        {
            Console.WriteLine("Select an option:");

            foreach (KeyValuePair<int, string> kvp in commandLineOptions)
            {
                int key = kvp.Key;
                string value = kvp.Value;

                Console.WriteLine($"{key}. {value}");
            }

            Console.Write("Enter you input: ");
        }

        static async Task Main(string[] args)
        {
            PrintCommandLine();
            if (!int.TryParse(Console.ReadLine(), out int input))
            {
                Console.WriteLine("Invalid input");
                Environment.Exit(0);
            }

            if (!commandLineOptions.TryGetValue(input, out string testName))
            {
                Console.WriteLine("Invalid option");
                Environment.Exit(0);
            }

            Console.WriteLine($"Executing {testName}");
            string instanceId = Guid.NewGuid().ToString();

            var eventListener = new ObservableEventListener();
            eventListener.LogToConsole(formatter: new DtfEventFormatter());
            eventListener.EnableEvents(DefaultEventSource.Log, EventLevel.Informational);

            if (string.IsNullOrEmpty(DtfTesterSettings.AzureStorageConnectionString))
            {
                Console.WriteLine("Azure Storage Connection String is empty, please provide valid connection string");
                Environment.Exit(0);
            }

            // Uncomment LoggerFactory if you want logs from Azure.Storage
            var settings = new AzureStorageOrchestrationServiceSettings
            {
                StorageAccountDetails = new StorageAccountDetails { ConnectionString = DtfTesterSettings.AzureStorageConnectionString },
                TaskHubName = DtfTesterSettings.TaskHubName,
                // LoggerFactory = LoggerFactory.Create(builder => builder.AddConsole()),
            };

            var orchestrationServiceAndClient = new AzureStorageOrchestrationService(settings);
            Console.WriteLine(orchestrationServiceAndClient.ToString());
            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient);
            var taskHubWorker = new TaskHubWorker(orchestrationServiceAndClient);
            taskHubWorker.ErrorPropagationMode = ErrorPropagationMode.SerializeExceptions;

            await orchestrationServiceAndClient.CreateIfNotExistsAsync();

            try {
                InitializeTests(instanceId, taskHubClient);
                var testMethod = testInstances[testName];
                var instance = await testMethod();
                Console.WriteLine("Workflow Instance Started: " + instance);
                Console.WriteLine("Initializing worker");
                await InitializeTaskWorker(taskHubWorker);

                int timeout = 5;
                Console.WriteLine($"Waiting up to {timeout} minutes for completion.");

                OrchestrationState taskResult = await taskHubClient.WaitForOrchestrationAsync(instance, TimeSpan.FromMinutes(timeout), CancellationToken.None);
                Console.WriteLine($"Task done: {taskResult?.OrchestrationStatus}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await taskHubWorker.StopAsync(true);
            }

        }
    }
}
