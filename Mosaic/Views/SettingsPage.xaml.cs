// ------------------------------------------------------------------------------
// <copyright file="SettingsPage.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Views
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Windows.System;

    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        public string Version
        {
            get
            {
                var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
                return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        public string AppTitle
        {
            get
            {
                var window = (Application.Current as App)?.StartupWindow;
                return window?.GetAppTitleFromSystem() ?? string.Empty;
            }
        }

        private async void BugRequestCard_Click(object sender, RoutedEventArgs e)
            => await Launcher.LaunchUriAsync(new Uri("https://github.com/roryclaasen/Mosaic/issues/new/choose"));
    }
}
