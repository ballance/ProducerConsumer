using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class Producer
    {
        private Random randy;
        public Producer()
        {
            randy = new Random();
        }

        public void Produce(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(randy.Next(1, 2000));
                var transferItem = new TransferInitializer()
                {
                    Id = Guid.NewGuid(),
                    Name = "One Production Item",
                    TransferUrl = "http://google.com"
                };
                EnqueueTransfer(transferItem);
            }
        }

        private void EnqueueTransfer(TransferInitializer transferItem)
        {
            TransferQueue.Instance.Add(transferItem);
            Console.WriteLine("{2} -------> Produced {0} - {1}. [thread #{3}]", transferItem.Id, transferItem.Name, TransferQueue.Count.ToString().PadLeft(3), System.Threading.Thread.CurrentThread.ManagedThreadId);
        }
    }
}
