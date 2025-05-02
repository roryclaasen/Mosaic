// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic;

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Mosaic.Controls;
using Windows.ApplicationModel;
using Windows.Foundation.Metadata;

public sealed partial class MainWindow : Window
{
    public MainWindow() => this.InitializeComponent();

    public void SetAppTitleBar()
    {
        this.Title = this.GetAppTitleFromSystem();
        if (AppWindowTitleBar.IsCustomizationSupported())
        {
            this.ExtendsContentIntoTitleBar = true;
            this.SetTitleBar(this.AppTitleBar);
        }
        else
        {
            this.AppTitleBar.Visibility = Visibility.Collapsed;
        }
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public string GetAppTitleFromSystem()
    {
#if MICROSOFT_WINDOWSAPPSDK_SELFCONTAINED
        var title = "Mosaic Video Viewer";
#else
        var title = Package.Current.DisplayName;
#endif

#if DEBUG
        title += " (DEBUG)";
#endif
        return title;
    }

    private async void myButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AboutDialog();
        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
        {
            dialog.XamlRoot = this.Content.XamlRoot;
        }

        await dialog.ShowAsync();
    }
}
