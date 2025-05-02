// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Helper;

using System.Runtime.InteropServices;

#pragma warning disable SA1307, SA1201
internal static class NativeMethods
{
    #region CoreMessaging.dll

    [DllImport("CoreMessaging.dll")]
    public static extern int CreateDispatcherQueueController([In] DispatcherQueueOptions options, [In, Out, MarshalAs(UnmanagedType.IUnknown)] ref object dispatcherQueueController);

    #endregion

    [StructLayout(LayoutKind.Sequential)]
    public struct DispatcherQueueOptions
    {
        public int dwSize;
        public int threadType;
        public int apartmentType;
    }
}
#pragma warning restore SA1307, SA1201

