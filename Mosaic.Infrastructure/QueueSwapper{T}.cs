// ------------------------------------------------------------------------------
// <copyright file="QueueSwapper{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public class QueueSwapper<T> : IQueueSwapper<T>
        where T : class
    {
        private readonly int gridSize;
        private readonly Queue<T> entries;

        public QueueSwapper(int gridSize, IEnumerable<T> entries)
            : this(gridSize, new Queue<T>(entries))
        {
        }

        public QueueSwapper(int gridSize, Queue<T> entries)
        {
            this.gridSize = gridSize;
            this.entries = entries;
        }

        public int NextSwapIndex { get; private set; } = 0;

        public bool TryDequeue(out T result) => this.entries.TryDequeue(out result);

        public T Swap(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (this.CanSwap())
            {
                this.entries.Enqueue(item);

                this.NextSwapIndex++;
                if (this.NextSwapIndex >= this.gridSize)
                {
                    this.NextSwapIndex = 0;
                }

                return this.entries.Dequeue();
            }

            return null;
        }

        public bool CanSwap() => this.entries.TryPeek(out _);
    }
}
