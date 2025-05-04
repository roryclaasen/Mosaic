// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Events;

using System;
using Microsoft.UI.Windowing;
using Mosaic.Util;

internal class WindowCreatedEventArgs(MainWindow window) : EventArgs
{
    public MainWindow Window { get; } = window;

    public AppWindow AppWindow => AppWindow.GetFromWindowId(WindowHelper.GetWindowIdFromCurrentWindow(this.Window));
}
