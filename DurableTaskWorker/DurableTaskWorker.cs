using DurableTask.Core;
using DurableTask.Core.Tracing;
using DurableTaskSamples.Common.Logging;
using DurableTaskSamples.Common.Utils;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace DurableTaskSamples.DurableTaskWorker
{
    internal class DurableTaskWorker
    {
        private TaskHubWorker taskHubWorker;

        private async Task InitializeTaskWorkerAsync()
        {
            this.taskHubWorker.AddTaskOrchestrations(
                typeof(SameActivityMultipleSchedulesOrchestration),
                typeof(MultipleActivitiesOrchestration),
                typeof(ContinueAsNewTestingOrchestration),
                typeof(ErrorHandlingWithContinueAsNewOrchestration),
                typeof(InlineForLoopTestingOrchestration),
                typeof(ErrorHandlingWithInlineRetriesOrchestration),
                typeof(FixedPollingWithInlineRetriesOrchestration),
                typeof(UnboundedPollingWithInlineRetriesOrchestration),
                typeof(UnboundedPollingWithContinueAsNewOrchestration));
            this.taskHubWorker.AddTaskActivities(
                new GreetingActivity(),
                new FirstActivity(),
                new SecondActivity(),
                new RetryableExceptionThrowingActivity(),
                new PollingActivity(),
                new AlwaysThrowingActivity());
            await this.taskHubWorker.StartAsync();
        }

        public async Task Start()
        {
            if (Utils.ShouldLogDtfCoreTraces())
            {
                var eventListener = new ObservableEventListener();
                eventListener.LogToConsole(formatter: new DtfEventFormatter());
                eventListener.EnableEvents(DefaultEventSource.Log, EventLevel.Informational);
            }

            var orchestrationServiceAndClient = Utils.GetAzureOrchestrationServiceClient();
            Console.WriteLine(orchestrationServiceAndClient.ToString());
            this.taskHubWorker = new TaskHubWorker(orchestrationServiceAndClient);
            this.taskHubWorker.ErrorPropagationMode = ErrorPropagationMode.SerializeExceptions;

            await this.InitializeTaskWorkerAsync();
        }

        public async Task Stop()
        {
            await this.taskHubWorker.StopAsync(true);
        }

    }
}
