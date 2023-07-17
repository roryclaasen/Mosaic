// ------------------------------------------------------------------------------
// <copyright file="QueueSwapper{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class QueueSwapper<T> : IQueueSwapper<T>
        where T : class
    {
        private const int DefaultActiveLength = 9;
        private readonly Queue<T> entries;

        public QueueSwapper(IEnumerable<T> entries, int activeLength = DefaultActiveLength)
            : this(new Queue<T>(entries), activeLength)
        {
        }

        public QueueSwapper(Queue<T> entries, int activeLength = DefaultActiveLength)
        {
            if (activeLength <= 1)
            {
                throw new ArgumentOutOfRangeException(nameof(activeLength), "Active length must be greater than 1");
            }

            this.ActiveLength = activeLength;
            this.entries = entries;
        }

        public int ActiveLength { get; set; }

        public int NextSwapIndex { get; private set; } = 0;

        public bool TryDequeue([MaybeNullWhen(false)] out T result) => this.entries.TryDequeue(out result);

        public void Enqueue(T item) => this.entries.Enqueue(item);

        public T? Swap(T activeItem)
        {
            if (activeItem == null)
            {
                throw new ArgumentNullException(nameof(activeItem));
            }

            if (this.CanSwap())
            {
                this.entries.Enqueue(activeItem);

                this.NextSwapIndex++;
                if (this.NextSwapIndex >= this.ActiveLength)
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
