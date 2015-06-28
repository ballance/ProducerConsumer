using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ProducerConsumer
{
    public class TransferQueue
    {
        private static readonly Lazy<BlockingCollection<TransferInitializer>> lazy = new Lazy<BlockingCollection<TransferInitializer>>(() => new BlockingCollection<TransferInitializer>(new ConcurrentQueue<TransferInitializer>(), 25));

        public static BlockingCollection<TransferInitializer> Instance => lazy.Value;

        public static int Count => Instance.Count;
    }
}