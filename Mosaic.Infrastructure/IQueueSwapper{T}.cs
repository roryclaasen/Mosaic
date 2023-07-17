// ------------------------------------------------------------------------------
// <copyright file="IQueueSwapper{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System.Diagnostics.CodeAnalysis;

    public interface IQueueSwapper<T>
    {
        int ActiveLength { get; set; }

        int NextSwapIndex { get; }

        bool TryDequeue([MaybeNullWhen(false)] out T result);

        void Enqueue(T item);

        T? Swap(T activeItem);

        bool CanSwap();
    }
}
