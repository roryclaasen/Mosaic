// ------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mosaic
{
    using System.IO;
    using Microsoft.UI.Xaml;
    using Mosaic.Infrastructure;
    using Newtonsoft.Json;
    using Windows.ApplicationModel;

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private MosaicWindow window;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            this.window = new MosaicWindow();
            this.window.Activate();
            this.window.InitializeConfig(this.LoadConfig());
        }

        private MosaicApplicationConfig LoadConfig()
        {
            var file = this.GetConfigFilePath();
            var configLoader = new ConfigLoader<MosaicApplicationConfig>(new JsonSerializer());
            return configLoader.LoadConfigFile(file);
        }

        private string GetConfigFilePath()
        {
            var configFile = "config.json"; // TODO: Can we load it via other means?

            return Path.Combine(Package.Current.InstalledLocation.Path, configFile);
        }
    }
}
