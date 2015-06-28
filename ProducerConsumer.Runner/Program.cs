using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer.Runner
{
    class Program
    {
        private const int NumberOfProducers = 7;
        private const int NumberOfConsumers = 6;

        private static bool producersAreRunning = false;
        private static bool consumersAreRunning = false;

        private static CancellationTokenSource producerCancellationTokenSource = new CancellationTokenSource();
        private static CancellationTokenSource consumerCancellationTokenSource = new CancellationTokenSource();

        private static CancellationToken consumerToken = consumerCancellationTokenSource.Token;
        private static CancellationToken producerToken = producerCancellationTokenSource.Token;

        static bool completedRunFlag = false;

        private static void Main(string[] args)
        {
            Console.WriteLine(" ==================================================================================");
            Console.WriteLine(" Producer / Consumer Pattern implemented with BlockingCollection <ConcurrentQueue> ");
            Console.WriteLine(" ----------------------------------------------------------------------------------");
            Console.WriteLine(" Producer threads [{0}]          Consumer threads [{1}]         BlockingBoundary [25] ", NumberOfProducers.ToString().PadLeft(2), NumberOfConsumers.ToString().PadLeft(2));
            Console.WriteLine(" ==================================================================================");
            Console.WriteLine("                                                                Press ESC to close");
            Console.WriteLine();
            Console.WriteLine(" Press 'p' to stop/start producers or 'c' to stop/start consumers");
            Console.WriteLine();

            do
            {
                while (!Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.P)
                    {
                        if (producersAreRunning)
                        {
                            producerCancellationTokenSource.Cancel();
                            producersAreRunning = false;
                            Console.WriteLine();
                            Console.WriteLine(" Producers stopping...");
                            Console.WriteLine();
                        }
                        else
                        {
                            producerCancellationTokenSource = new CancellationTokenSource();
                            producerToken = producerCancellationTokenSource.Token;
                            StartProducers(producerToken);
                            Console.WriteLine();
                            Console.WriteLine(" Producers starting...");
                            Console.WriteLine();

                        }
                        break;
                    }
                    if (key.Key == ConsoleKey.C)
                    {
                        if (consumersAreRunning)
                        {
                            consumerCancellationTokenSource.Cancel(true);
                            consumersAreRunning = false;
                            Console.WriteLine();
                            Console.WriteLine(" Consumers stopping...");
                            Console.WriteLine();
                        }
                        else
                        {
                            consumerCancellationTokenSource = new CancellationTokenSource();
                            consumerToken = consumerCancellationTokenSource.Token;
                            StartConsumers(consumerToken);
                            Console.WriteLine();
                            Console.WriteLine(" Consumers starting...");
                            Console.WriteLine();
                        }
                        break;
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }

        private static void StartProducers(CancellationToken token)
        {
            var producer = new Producer();
            for (var j = 0; j < NumberOfProducers; j++)
            {
                Task.Run(
                    () => { producer.Produce(token); }, token);
            }
            producersAreRunning = true;
        }

        private static void StartConsumers(CancellationToken token)
        {
            var consumer = new Consumer();
            for (var i = 0; i < NumberOfConsumers; i++)
            {
                Task.Run(
                    () => { consumer.Consume(token); }, token);
            }
            consumersAreRunning = true;
        }
    }
}