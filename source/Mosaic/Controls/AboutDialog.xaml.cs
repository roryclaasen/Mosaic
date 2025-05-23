// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Controls;

using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.System;

public sealed partial class AboutDialog : ContentDialog
{
    public AboutDialog()
    {
        this.InitializeComponent();
    }

    public string ApplicationVersion => ThisAssembly.AssemblyFileVersion;

    public string ApplicationDisplayName
    {
        get
        {
            var window = (Application.Current as App)?.StartupWindow;
            return window?.GetAppTitleFromSystem() ?? string.Empty;
        }
    }

    private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        => await Launcher.LaunchUriAsync(new Uri("https://github.com/roryclaasen/Mosaic/issues/new/choose"));
}
