namespace Mosaic
{
    using System.Windows;

    public partial class App : Application
    {
        private string[] Args;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Args = e.Args;

            var view = new MosaicWindow();
            view.Show();
            view.InitializeSources(this.Args);
        }
    }
}
