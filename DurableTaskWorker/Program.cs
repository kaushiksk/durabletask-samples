namespace DurableTaskSamples.DurableTaskWorker
{
    using DurableTaskSamples.Common.Logging;
    using DurableTaskSamples.Common.Utils;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class Program
    {
        static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                if (Utils.ShouldDisableVerboseLogsInOrchestration())
                {
                    Logger.SetVerbosity(false);
                }
            }

            if (args.Length == 1)
            {
                if (args[0] == "disableVerboseLogs")
                {
                    Logger.SetVerbosity(false);
                }
            }

            Console.CancelKeyPress += (sender, eArgs) => {
                _quitEvent.Set();
                eArgs.Cancel = true;
            };

            var taskHubWorker = new DurableTaskWorker();
            try
            {
                Console.WriteLine("Initializing worker");
                await taskHubWorker.Start();

                Console.WriteLine("Started TaskhubWorker, press Ctrl-C to stop");
                _quitEvent.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                await taskHubWorker.Stop();
            }

        }
    }
}

