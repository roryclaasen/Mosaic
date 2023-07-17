// ------------------------------------------------------------------------------
// <copyright file="HomePage.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Views
{
    using System;
    using System.Collections.Generic;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Controls.Primitives;
    using Mosaic.Controls;
    using Mosaic.Infrastructure;
    using Windows.Foundation.Metadata;

    public sealed partial class HomePage : Page
    {
        private readonly MosaicManager mosaicManager;

        public HomePage()
        {
            this.mosaicManager = new MosaicManager();
            var sources = new List<SourceConfig>();
            for (var i = 0; i < 9; i++)
            {
                sources.Add(new SourceConfig
                {
                    Source = "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"
                });
            }

            this.mosaicManager.SetConfig(new MosaicConfig
            {
                Sources = sources
            });

            this.InitializeComponent();
        }

        private async void CommandBar_ShowAbout(object sender, RoutedEventArgs e)
        {
            var dialog = new AboutDialog();
            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8))
            {
                dialog.XamlRoot = this.XamlRoot;
            }

            await dialog.ShowAsync();
        }

        private void CommandBar_Play(object sender, RoutedEventArgs e)
            => this.MosaicGrid.Play();

        private void CommandBar_Stop(object sender, RoutedEventArgs e)
            => this.MosaicGrid.Stop();

        private void CommandBar_ChangeTheme(object sender, RoutedEventArgs e)
        {
            if (sender is RadioMenuFlyoutItem button && App.Current.StartupWindow?.Content is FrameworkElement frameworkElement)
            {
                frameworkElement.RequestedTheme = button.Tag switch
                {
                    "light" => ElementTheme.Light,
                    "dark" => ElementTheme.Dark,
                    _ => ElementTheme.Default,
                };
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
                    this.MosaicGrid.MosaicWidth = newValue;
                    break;
                case "m_height":
                    this.MosaicGrid.MosaicHeight = newValue;
                    break;
                default:
                    throw new Exception("Unknown slider tag");
            }
        }

        private void CommandBar_ToggleLabels(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch)
            {
                this.MosaicGrid.SetShowLabels(toggleSwitch.IsOn);
            }
        }
    }
}
