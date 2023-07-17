// ------------------------------------------------------------------------------
// <copyright file="IWeightedBag{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Collections
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public interface IWeightedBag<T>
    {
        int Count { get; }

        void Add(T item);

        void AddRange(IEnumerable<T> items);

        bool TryGetNext([MaybeNullWhen(false)] out T result);

        bool TryPeak([MaybeNullWhen(false)] out T result);

        void Clear();
    }
}
