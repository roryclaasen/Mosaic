// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic;

using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Mosaic.Util;

public partial class App : Application
{
    public App() => this.InitializeComponent();

    public MainWindow? Window { get; private set; }

    [MemberNotNull(nameof(this.Window))]
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        this.Window = new MainWindow();
        this.Window.SetAppTitleBar();
        this.Window.Activate();

        this.TrySetMicaBackdrop();
    }
}
