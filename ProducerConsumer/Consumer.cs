using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class Consumer
    {
        private Random randy;

        public Consumer()
        {
            randy = new Random();
        }

        public void Consume(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var found = false;

                TransferInitializer transferItem;
                found = TransferQueue.Instance.TryTake(out transferItem);
                if (found)
                {
                    Console.WriteLine("{2} <------- Consumed {0} - {1}. [thread# {3}]", transferItem.Id, transferItem.Name,
                        TransferQueue.Count.ToString().PadLeft(3, ' '), Thread.CurrentThread.ManagedThreadId);

                    Thread.Sleep(randy.Next(1, 2000));
                }
            }
        }


        //public void ConsumeOne()
        //{
        //    var found = false;
        //    while (!found)
        //    {
        //        TransferInitializer transferItem;
        //        found = TransferQueue.Instance.TryTake(out transferItem);
        //        Console.WriteLine("{2} <------- Consumed {0} - {1}.", transferItem.Id, transferItem.Name,
        //            TransferQueue.Count);
        //    }
        //}
    }
}
