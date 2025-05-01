// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Infrastructure.Collections;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents a thread-safe first-in, first-out collection of objects.
/// Objects will be added to the end of the queue when removed from the beginning.
/// </summary>
/// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
public interface IConcurrentLoopingQueue<T> : IEnumerable<T>
{
    /// <summary>
    /// Gets the number of items in the <see cref="IConcurrentLoopingQueue{T}"/>.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Adds an object to the end of the <see cref="IConcurrentLoopingQueue{T}"/>.
    /// </summary>
    /// <param name="item">The object to add to the end of the <see cref="IConcurrentLoopingQueue{T}"/>.</param>
    void Enqueue(T item);

    /// <summary>
    /// Adds a collection of objects to the end of the <see cref="IConcurrentLoopingQueue{T}"/>.
    /// </summary>
    /// <param name="items">The collection of objects to add to the end of the <see cref="IConcurrentLoopingQueue{T}"/>.</param>
    void EnqueueRange(IEnumerable<T> items);

    /// <summary>
    /// Attempts to remove and return the object at the beginning of the <see cref="IConcurrentLoopingQueue{T}"/>.
    /// This object will then be added to the end of the queue.
    /// </summary>
    /// <param name="result">
    /// When this method returns, if the operation was successful, <paramref name="result"/> contains the object removed.
    /// If no object was available to be removed, the value is unspecified.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if an element was removed and returned from the beginning of the <see cref="IConcurrentLoopingQueue{T}"/> successfully; otherwise, false.
    /// </returns>
    bool TryDequeue([MaybeNullWhen(false)] out T result);

    /// <summary>
    /// Peeks the next item in the queue without removing it.
    /// </summary>
    /// <param name="result">
    /// When this method returns, <paramref name="result"/> contains an object from
    /// the beginning of the <see cref="IConcurrentLoopingQueue{T}"/> or default(T)
    /// if the operation failed.
    /// </param>
    /// <returns><see langword="true"/> if and object was returned successfully; otherwise, <see langword="false"/>.</returns>
    bool TryPeak([MaybeNullWhen(false)] out T result);

    /// <summary>
    /// Removes all items from the <see cref="IConcurrentLoopingQueue{T}"/>.
    /// </summary>
    void Clear();
}
