using DurableTask.Core;
using DurableTaskSamples.Common.Utils;
using System;
using System.Threading.Tasks;

namespace DurableTaskManager
{
    internal class Program
    {
        enum WorkflowOperation
        {
            GetStatus = 1,
            Pause = 2,
            Resume = 3,
        };

        static async Task Main(string[] args)
        {
            var orchestrationServiceAndClient = Utils.GetAzureOrchestrationServiceClient();
            Console.WriteLine(orchestrationServiceAndClient.ToString());
            var taskHubClient = new TaskHubClient(orchestrationServiceAndClient);

            string instanceId;
            if (args.Length > 0)
            {
                instanceId = args[0];
                Utils.WriteToConsoleWithColor("Launching instance manager for instance: " + instanceId, ConsoleColor.Yellow);
            }
            else
            {
                Utils.WriteToConsoleWithColor("Enter InstanceId to manage:", ConsoleColor.Yellow);
                instanceId = Console.ReadLine();
            }

            var currentStatus = await taskHubClient.GetOrchestrationStateAsync(instanceId).ConfigureAwait(false);
            Utils.WriteToConsoleWithColor("Workflow Instance Status: " + currentStatus.OrchestrationStatus, ConsoleColor.Green);
            Console.WriteLine();

            while (true)
            {
                Utils.WriteToConsoleWithColor("Select an option:", ConsoleColor.Yellow);
                Console.WriteLine("1. Get Workflow Instance Status");
                Console.WriteLine("2. Pause Workflow Instance");
                Console.WriteLine("3. Resume Workflow Instance");
                Console.WriteLine("4. Exit");
                Utils.WriteToConsoleWithColor("Enter your input:", ConsoleColor.Yellow);
                if (!int.TryParse(Console.ReadLine(), out int input))
                {
                    Console.WriteLine("Invalid input");
                    Environment.Exit(0);
                }

                if (input == 4)
                {
                    Environment.Exit(0);
                }

                if (!Enum.TryParse(input.ToString(), out WorkflowOperation operation))
                {
                    Console.WriteLine("Invalid input");
                    continue;
                }

                switch (operation)
                {
                    case WorkflowOperation.GetStatus:
                        {
                            var status = await taskHubClient.GetOrchestrationStateAsync(instanceId).ConfigureAwait(false);
                            Utils.WriteToConsoleWithColor("Workflow Instance Status: " + status.OrchestrationStatus, ConsoleColor.Green);
                        }
                        break;
                    case WorkflowOperation.Pause:
                        {
                            Utils.WriteToConsoleWithColor("Pausing instance", ConsoleColor.Yellow);

                            var instance = new OrchestrationInstance() { InstanceId = instanceId };
                            await taskHubClient.SuspendInstanceAsync(instance).ConfigureAwait(false);

                            Utils.WriteToConsoleWithColor("Instance Paused, please check status after few seconds", ConsoleColor.Yellow);
                        }
                        break;
                    case WorkflowOperation.Resume:
                        {
                            Utils.WriteToConsoleWithColor("Resuming instance", ConsoleColor.Yellow);

                            var instance = new OrchestrationInstance() { InstanceId = instanceId };
                            await taskHubClient.ResumeInstanceAsync(instance).ConfigureAwait(false);

                            Utils.WriteToConsoleWithColor("Instance resumed, please check status after few seconds", ConsoleColor.Yellow);

                        }
                        break;

                    default:
                        break;
                }
                Console.WriteLine();
            }
        }
    }
}
