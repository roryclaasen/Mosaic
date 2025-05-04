// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic;

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml;
using Mosaic.Events;

public partial class App : Application
{
    public App() => this.InitializeComponent();

    internal event EventHandler<WindowCreatedEventArgs>? WindowCreated;

    /// <inheritdoc cref="Application.Current"/>
    public static new App Current => (App)Application.Current;

    public MainWindow? Window { get; private set; }

    [MemberNotNull(nameof(this.Window))]
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        this.Window = new MainWindow();
        this.Window.SetAppTitleBar();
        this.Window.Activate();

        this.TrySetMicaBackdrop();

        this.WindowCreated?.Invoke(this, new WindowCreatedEventArgs(this.Window));
    }
}
