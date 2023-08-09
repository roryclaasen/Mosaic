// ------------------------------------------------------------------------------
// <copyright file="ConcurrentLoopingQueue{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Collections
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class ConcurrentLoopingQueue<T> : IConcurrentLoopingQueue<T>
    {
        private readonly ConcurrentQueue<T> entries = new();

        public int Count => this.entries.Count;

        public void Enqueue(T item) => this.entries.Enqueue(item);

        public void EnqueueRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Enqueue(item);
            }
        }

        public bool TryDequeue([MaybeNullWhen(false)] out T result)
        {
            if (this.entries.TryDequeue(out result))
            {
                this.entries.Enqueue(result);
                return true;
            }

            return false;
        }

        public bool TryPeak([MaybeNullWhen(false)] out T result)
            => this.entries.TryPeek(out result);

        public void Clear()
            => this.entries.Clear();
    }
}
