// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Helper;

using System.Runtime.InteropServices;
using Windows.System;

internal class WindowsSystemDispatcherQueueHelper
{
    private object? dispatcherQueueController = null;

    public void EnsureWindowsSystemDispatcherQueueController()
    {
        if (DispatcherQueue.GetForCurrentThread() is not null)
        {
            return;
        }

        if (this.dispatcherQueueController is null)
        {
            NativeMethods.DispatcherQueueOptions options;
            options.dwSize = Marshal.SizeOf<NativeMethods.DispatcherQueueOptions>();
            options.threadType = 2;    // DQTYPE_THREAD_CURRENT
            options.apartmentType = 2; // DQTAT_COM_STA

#pragma warning disable CS8601 // Possible null reference assignment.
            NativeMethods.CreateDispatcherQueueController(options, ref this.dispatcherQueueController);
#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }
}
