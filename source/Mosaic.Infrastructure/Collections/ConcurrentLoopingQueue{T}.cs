// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Infrastructure.Collections;

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <inheritdoc/>
public class ConcurrentLoopingQueue<T> : IConcurrentLoopingQueue<T>
{
#if NET9_0_OR_GREATER
    private readonly Lock lockObject = new();
#else
    private readonly object lockObject = new();
#endif

    private readonly ConcurrentQueue<T> entries = new();

    /// <inheritdoc/>
    public int Count => this.entries.Count;

    /// <inheritdoc/>
    public void Enqueue(T item)
    {
        lock (this.lockObject)
        {
            this.entries.Enqueue(item);
        }
    }

    /// <inheritdoc/>
    public void EnqueueRange(IEnumerable<T> items)
    {
        lock (this.lockObject)
        {
            foreach (var item in items)
            {
                this.Enqueue(item);
            }
        }
    }

    /// <inheritdoc/>
    public bool TryDequeue([MaybeNullWhen(false)] out T result)
    {
        lock (this.lockObject)
        {
            if (this.entries.TryDequeue(out result))
            {
                this.entries.Enqueue(result);
                return true;
            }

            return false;
        }
    }

    /// <inheritdoc/>
    public bool TryPeak([MaybeNullWhen(false)] out T result)
        => this.entries.TryPeek(out result);

    /// <inheritdoc/>
    public void Clear()
    {
        lock (this.lockObject)
        {
            this.entries.Clear();
        }
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => this.entries.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}
