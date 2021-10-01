namespace Mosaic
{
    using System;
    using System.Windows;

    public partial class App : Application
    {
        private string[] Args;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0) throw new ArgumentException($"{nameof(e.Args)} cannot be empty", nameof(e.Args));

            this.Args = e.Args;
            // TODO Validate Args
            // Prompt of Args

            this.CreateAndShowMosaic();
        }

        private void CreateAndShowMosaic()
        {
            var view = new MosaicWindow();
            view.Show();
            view.InitializeSources(this.Args);
        }
    }
}
