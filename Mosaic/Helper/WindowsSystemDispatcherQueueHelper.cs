// ------------------------------------------------------------------------------
// <copyright file="WindowsSystemDispatcherQueueHelper.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Helper
{
    using System.Runtime.InteropServices;

    internal class WindowsSystemDispatcherQueueHelper
    {
        private object dispatcherQueueController = null;

        public void EnsureWindowsSystemDispatcherQueueController()
        {
            if (Windows.System.DispatcherQueue.GetForCurrentThread() is not null)
            {
                return;
            }

            if (this.dispatcherQueueController is null)
            {
                DispatcherQueueOptions options;
                options.dwSize = Marshal.SizeOf(typeof(DispatcherQueueOptions));
                options.threadType = 2;    // DQTYPE_THREAD_CURRENT
                options.apartmentType = 2; // DQTAT_COM_STA

                CreateDispatcherQueueController(options, ref this.dispatcherQueueController);
            }
        }

        [DllImport("CoreMessaging.dll")]
        private static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

        [StructLayout(LayoutKind.Sequential)]
        private struct DispatcherQueueOptions
        {
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
            internal int dwSize;
            internal int threadType;
            internal int apartmentType;
#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
        }
    }
}
