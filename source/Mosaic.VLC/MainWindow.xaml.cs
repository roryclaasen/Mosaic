// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic;

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

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

    public string GetAppTitleFromSystem()
    {
#if MICROSOFT_WINDOWSAPPSDK_SELFCONTAINED
        var title = "Mosaic Video Viewer";
#else
        var title = Windows.ApplicationModel.Package.Current.DisplayName;
#endif

#if DEBUG
        title += " - Dev";
#endif
        return title;
    }
}
