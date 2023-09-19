namespace DurableTaskClient
{
    using System.Threading.Tasks;

    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                await CommandLineClient.Start();
            }
        }
    }
}

