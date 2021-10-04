// ------------------------------------------------------------------------------
// <copyright file="IQueueSwapper{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    public interface IQueueSwapper<T>
    {
        int NextSwapIndex { get; }

        bool TryDequeue(out T result);

        T Swap(T item);

        bool CanSwap();
    }
}
