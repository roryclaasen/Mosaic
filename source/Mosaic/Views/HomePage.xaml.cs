// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Views;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Mosaic.Controls;
using Mosaic.Infrastructure;
using Mosaic.Infrastructure.Config;
using Mosaic.Util;
using Windows.Foundation.Metadata;
using Windows.Storage.Pickers;
using WinRT.Interop;

public sealed partial class HomePage : Page
{
    private readonly MosaicManager mosaicManager;
    private readonly CsvConfiguration csvConfiguration;

    public HomePage()
    {
        this.mosaicManager = new MosaicManager();
        this.csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            IgnoreBlankLines = true
        };

        this.InitializeComponent();
    }

    private async void CommandBar_ShowAbout(object sender, RoutedEventArgs e)
    {
        var dialog = new AboutDialog
        {
            RequestedTheme = this.RequestedTheme
        };

        if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
        {
            dialog.XamlRoot = this.XamlRoot;
        }

        await dialog.ShowAsync();
    }

    private async void CommandBar_OpenFile(object sender, RoutedEventArgs e)
    {
        var filePicker = new FileOpenPicker
        {
            ViewMode = PickerViewMode.List,
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            FileTypeFilter = { ".csv" }
        };

        InitializeWithWindow.Initialize(filePicker, WindowHelper.GetWindowHandleForCurrentWindow(App.Current.Window!));

        var file = await filePicker.PickSingleFileAsync();
        if (file is not null)
        {
            using var steamReader = new StreamReader(await file.OpenStreamForReadAsync());
            using var csvReader = new CsvReader(steamReader, this.csvConfiguration);

            this.SetVideoSources(csvReader.GetRecords<MediaEntry>());
        }
    }

    private void CommandBar_LoadExampleVideos(object sender, RoutedEventArgs e) => this.SetVideoSources([
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ElephantsDream.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerBlazes.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerEscapes.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerFun.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerJoyrides.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/ForBiggerMeltdowns.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/Sintel.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/SubaruOutbackOnStreetAndDirt.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/TearsOfSteel.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/VolkswagenGTIReview.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/WeAreGoingOnBullrun.mp4",
        "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/WhatCarCanYouGetForAGrand.mp4"
    ]);

    private void CommandBar_Play(object sender, RoutedEventArgs e)
    {
        if (this.mosaicManager.SourceCount > 0)
        {
            this.MosaicGrid.Play();
        }
    }

    private void CommandBar_Stop(object sender, RoutedEventArgs e)
        => this.MosaicGrid.Stop();

    private void CommandBar_ChangeTheme(object sender, RoutedEventArgs e)
    {
        if (sender is RadioMenuFlyoutItem button)
        {
            this.RequestedTheme = button.Tag switch
            {
                "light" => ElementTheme.Light,
                "dark" => ElementTheme.Dark,
                _ => ElementTheme.Default,
            };

            if (App.Current.Window?.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = this.RequestedTheme;
            }
        }
    }

    private void CommandBar_SizeChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        var oldValue = (int)e.OldValue;
        var newValue = (int)e.NewValue;
        if (oldValue == newValue || this.MosaicGrid is null || sender is not Slider slider)
        {
            return;
        }

        switch (slider.Tag)
        {
            case "m_width":
                if (this.MosaicGrid.MosaicWidth != newValue)
                {
                    this.MosaicGrid.MosaicWidth = newValue;
                }

                break;
            case "m_height":
                if (this.MosaicGrid.MosaicHeight != newValue)
                {
                    this.MosaicGrid.MosaicHeight = newValue;
                }

                break;
            default:
                throw new Exception("Unknown slider tag");
        }
    }

    private void CommandBar_ToggleLabels(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch toggleSwitch)
        {
            this.MosaicGrid?.SetShowLabels(toggleSwitch.IsOn);
        }
    }

    private void CommandBar_ToggleMute(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleSwitch toggleSwitch)
        {
            this.MosaicGrid?.SetMute(toggleSwitch.IsOn);
        }
    }

    private void SetVideoSources(IEnumerable<string> entries)
        => this.SetVideoSources(entries.Select(x => new MediaEntry(new Uri(x))));

    private void SetVideoSources(IEnumerable<MediaEntry> entries)
    {
        var wasPlaying = this.MosaicGrid.IsPlaying;
        if (wasPlaying)
        {
            this.MosaicGrid.Stop();
        }

        this.mosaicManager.SetConfig(entries);
        this.MosaicGrid.Play();
    }
}
