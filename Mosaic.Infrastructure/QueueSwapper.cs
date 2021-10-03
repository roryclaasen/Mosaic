namespace Mosaic.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public sealed class QueueSwapper<T> where T : class
    {
        private readonly int GridSize;
        private readonly Queue<T> Entries;

        public int NextSwapIndex { get; private set; } = 0;

        public QueueSwapper(int gridSize, IEnumerable<T> entries)
            : this(gridSize, new Queue<T>(entries))
        {
        }

        public QueueSwapper(int gridSize, Queue<T> entries)
        {
            this.GridSize = gridSize;
            this.Entries = entries;
        }

        public bool TryDequeue(out T result) => this.Entries.TryDequeue(out result);

        public T Swap(T item)
        {
            if (this.Entries.Count == 0)
            {
                return null;
            }

            this.Entries.Enqueue(item);

            this.NextSwapIndex++;
            if (this.NextSwapIndex >= this.GridSize) this.NextSwapIndex = 0;
            return this.Entries.Dequeue();
        }
    }
}
