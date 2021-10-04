// ------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic
{
    using System;
    using System.Linq;
    using System.Windows;
    using Mosaic.Infrastructure;
    using Newtonsoft.Json;

    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.CreateAndShowMosaic(e.Args.FirstOrDefault() ?? "config.json");
        }

        private void CreateAndShowMosaic(string file)
        {
            var config = this.LoadConfig(file);

            var view = new MosaicWindow();
            view.Show();
            view.InitializeConfig(config);
        }

        private MosaicApplicationConfig LoadConfig(string file)
        {
            var configLoader = new ConfigLoader<MosaicApplicationConfig>(new JsonSerializer());
            return configLoader.LoadConfigFile(file);
        }
    }
}
