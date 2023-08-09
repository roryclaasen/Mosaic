// ------------------------------------------------------------------------------
// <copyright file="AboutDialog.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Controls
{
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

        public string ApplicationVersion
        {
            get
            {
                var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
                return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }

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
}
