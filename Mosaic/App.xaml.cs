namespace Mosaic
{
    using System;
    using System.Linq;
    using System.Windows;
    using Mosaic.Infrastructure;
    using Newtonsoft.Json;

    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e) => this.CreateAndShowMosaic();
        
        private void CreateAndShowMosaic()
        {
            var configLoader = new ConfigLoader<ApplicationConfig>(new JsonSerializer());
            var config = configLoader.LoadConfigFile("config.json");
            // TODO Catch any file errors

            var view = new MosaicWindow();
            view.Show();
            view.InitializeConfig(config);
        }
    }
}
