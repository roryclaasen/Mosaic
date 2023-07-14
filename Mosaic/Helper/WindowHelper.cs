// ------------------------------------------------------------------------------
// <copyright file="WindowHelper.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Helper
{
    using System;
    using Microsoft.UI;

    internal static partial class WindowHelper
    {
        public static IntPtr GetWindowHandleForCurrentWindow(object target)
        {
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(target);
            return hWnd;
        }

        public static WindowId GetWindowIdFromCurrentWindow(object target)
        {
            var wndId = Win32Interop.GetWindowIdFromWindow(GetWindowHandleForCurrentWindow(target));
            return wndId;
        }
    }
}
