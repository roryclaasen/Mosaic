// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Events;

using System;

internal class WindowCreatedEventArgs(MainWindow window) : EventArgs
{
    public MainWindow Window { get; } = window;
}
