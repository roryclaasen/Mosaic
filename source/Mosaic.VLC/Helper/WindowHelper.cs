// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Helper;

using System;
using Microsoft.UI;
using WinRT.Interop;

internal static partial class WindowHelper
{
    public static IntPtr GetWindowHandleForCurrentWindow(object target)
    {
        var hWnd = WindowNative.GetWindowHandle(target);
        return hWnd;
    }

    public static WindowId GetWindowIdFromCurrentWindow(object target)
    {
        var wndId = Win32Interop.GetWindowIdFromWindow(GetWindowHandleForCurrentWindow(target));
        return wndId;
    }
}
