// ------------------------------------------------------------------------------
// <copyright file="IConcurrentLoopingQueue{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface IConcurrentLoopingQueue<T>
    {
        int Count { get; }

        void Enqueue(T item);

        void EnqueueRange(IEnumerable<T> items);

        bool TryDequeue([MaybeNullWhen(false)] out T result);

        bool TryPeak([MaybeNullWhen(false)] out T result);

        void Clear();
    }
}
