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
        private const int NumberOfProducers = 14;
        private const int NumberOfConsumers = 2;

        private static bool producersAreRunning = false;
        private static bool consumersAreRunning = false;

        private static CancellationTokenSource producerCancellationTokenSource = new CancellationTokenSource();
        private static CancellationTokenSource consumerCancellationTokenSource = new CancellationTokenSource();

        private static CancellationToken consumerToken = consumerCancellationTokenSource.Token;
        private static CancellationToken producerToken = producerCancellationTokenSource.Token;

        static bool completedRunFlag = false;

        private static void Main(string[] args)
        {
            Console.WriteLine("Press ESC to close");
            Console.WriteLine("Press 'p' to stop/start producers or 'c' to stop/start consumers");
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
                            Console.WriteLine("Producers cancelled.");
                        }
                        else
                        {
                            producerCancellationTokenSource = new CancellationTokenSource();
                            producerToken = producerCancellationTokenSource.Token;
                            StartProducers(producerToken);
                            Console.WriteLine("Producers started.");

                        }
                        break;
                    }
                    if (key.Key == ConsoleKey.C)
                    {
                        if (consumersAreRunning)
                        {
                            consumerCancellationTokenSource.Cancel(true);
                            consumersAreRunning = false;
                            Console.WriteLine("Consumers cancelled.");
                        }
                        else
                        {
                            consumerCancellationTokenSource = new CancellationTokenSource();
                            consumerToken = consumerCancellationTokenSource.Token;
                            StartConsumers(consumerToken);
                            Console.WriteLine("Consumers started.");
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