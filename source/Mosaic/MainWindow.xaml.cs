// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic;

using System;
using System.Diagnostics.CodeAnalysis;
using FlyleafLib;
using FlyleafLib.Controls.WinUI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.ApplicationModel;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        Engine.Start(new EngineConfig
        {
            // TODO: Add an option to somehow use a user-defined path for ffmpeg
            FFmpegPath = @"D:\tools\ffmpeg\bin",

            // FFmpegDevices = false,

#if DEBUG
            FFmpegLogLevel = Flyleaf.FFmpeg.LogLevel.Warn,
            LogLevel = LogLevel.Debug,
            LogOutput = ":debug",
#else
            FFmpegLogLevel = Flyleaf.FFmpeg.LogLevel.Quiet,
            LogLevel = LogLevel.Quiet,
#endif

            UIRefresh = false
        });


        FullScreenContainer.CustomizeFullScreenWindow += FullScreenContainer_CustomizeFullScreenWindow;

        this.InitializeComponent();
    }

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

    private void FullScreenContainer_CustomizeFullScreenWindow(object sender, EventArgs e)
    {
        if (FullScreenContainer.FSWApp is not null)
        {
            FullScreenContainer.FSWApp.Title = this.Title + " (FS)";
        }

        if (FullScreenContainer.FSW is not null)
        {
            FullScreenContainer.FSW.Closed += (o, e) => this.Close();
        }
    }
}
