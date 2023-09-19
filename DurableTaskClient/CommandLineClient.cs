using DurableTask.Core;
using DurableTaskSamples;
using DurableTaskSamples.Common.Utils;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DurableTaskClient
{
    public class CommandLineClient
    {
        private static readonly Dictionary<int, Type> commandLineOptions = new Dictionary<int, Type>()
        {
            { 1, typeof(SameActivityMultipleSchedulesOrchestration)},
            { 2, typeof(MultipleActivitiesOrchestration)},
            { 3, typeof(ContinueAsNewTestingOrchestration)},
            { 4, typeof(ErrorHandlingWithContinueAsNewOrchestration)},
            { 5, typeof(InlineForLoopTestingOrchestration)},
            { 6, typeof(ErrorHandlingWithInlineRetriesOrchestration)},
            { 7, typeof(FixedPollingWithInlineRetriesOrchestration)},
            { 8, typeof(UnboundedPollingWithInlineRetriesOrchestration)},
            { 9, typeof(UnboundedPollingWithContinueAsNewOrchestration)},
        };

        private static readonly Dictionary<Type, object> orchestrationInputs = new Dictionary<Type, object>()
        {
            { typeof(SameActivityMultipleSchedulesOrchestration), 1},
            { typeof(MultipleActivitiesOrchestration), 1},
            { typeof(ContinueAsNewTestingOrchestration), 0},
            { typeof(ErrorHandlingWithContinueAsNewOrchestration), 0},
            { typeof(InlineForLoopTestingOrchestration), 5},
            { typeof(ErrorHandlingWithInlineRetriesOrchestration), 5},
            { typeof(FixedPollingWithInlineRetriesOrchestration), 10},
            { typeof(UnboundedPollingWithInlineRetriesOrchestration), 0},
            { typeof(UnboundedPollingWithContinueAsNewOrchestration), 0},
        };

        private static void PrintCommandLine()
        {
            WriteToConsoleWithColor("Select an option:", ConsoleColor.Yellow);

            foreach (KeyValuePair<int, Type> kvp in commandLineOptions)
            {
                int key = kvp.Key;
                string value = kvp.Value.Name;

                Console.WriteLine($"{key}. {value}");
            }

            WriteToConsoleWithColor("Enter you input: ", ConsoleColor.Yellow);
        }

        public static async Task Start()
        {
            PrintCommandLine();
            if (!int.TryParse(Console.ReadLine(), out int input))
            {
                Console.WriteLine("Invalid input");
                Environment.Exit(0);
            }

            if (!commandLineOptions.TryGetValue(input, out Type orchestrationSample))
            {
                Console.WriteLine("Invalid option");
                Environment.Exit(0);
            }

            Console.WriteLine($"Executing {orchestrationSample.Name}");
            string instanceId = Guid.NewGuid().ToString();

            var orchestrationServiceAndClient = Utils.GetAzureOrchestrationServiceClient();
            Console.WriteLine(orchestrationServiceAndClient.ToString());
            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient);

            try
            {
                var orchestrationInput = orchestrationInputs[orchestrationSample];
                var instance = await taskHubClient.CreateOrchestrationInstanceAsync(orchestrationSample, instanceId, orchestrationInput);
                Console.WriteLine("Workflow Instance Started: " + instance);

                int timeout = 5;
                Console.WriteLine($"Waiting up to {timeout} minutes for completion.");

                OrchestrationState taskResult = await taskHubClient.WaitForOrchestrationAsync(instance, TimeSpan.FromMinutes(timeout), CancellationToken.None);
                WriteToConsoleWithColor($"Task done. Orchestration status: {taskResult?.OrchestrationStatus}", ConsoleColor.Green);
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void WriteToConsoleWithColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
