// ------------------------------------------------------------------------------
// <copyright file="WeightedBag{T}.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Collections
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public class WeightedBag<T> : IWeightedBag<T>
    {
        private readonly ConcurrentBag<WeightedItem> entries;

        public WeightedBag()
        {
            this.entries = new ConcurrentBag<WeightedItem>();
        }

        public int Count => this.entries.Count;

        public void Add(T item) => this.entries.Add(new WeightedItem(item));

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        public bool TryGetNext([MaybeNullWhen(false)] out T result)
        {
            if (this.TryPeakWeightedItem(out var weightItem))
            {
                weightItem.Weight++;
                result = weightItem.Item;
                return true;
            }

            result = default;
            return false;
        }

        public bool TryPeak([MaybeNullWhen(false)] out T result)
        {
            if (this.TryPeakWeightedItem(out var weightItem))
            {
                result = weightItem.Item;
                return true;
            }

            result = default;
            return false;
        }

        public void Clear()
            => this.entries.Clear();

        private bool TryPeakWeightedItem([MaybeNullWhen(false)] out WeightedItem result)
        {
            var lowestWeight = this.entries.Min(x => x.Weight);
            result = this.entries.FirstOrDefault(x => x.Weight == lowestWeight);
            return result is not null;
        }

        private record WeightedItem
        {
            public WeightedItem(T item)
                : this(item, 0)
            {
            }

            public WeightedItem(T item, int weight)
            {
                this.Item = item;
                this.Weight = weight;
            }

            public T Item { get; init; }

            public int Weight { get; set; }
        }
    }
}
