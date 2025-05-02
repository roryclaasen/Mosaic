// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic;

using Microsoft.UI.Composition;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Mosaic.Helper;
using WinRT;

public sealed partial class App : Application
{
    private WindowsSystemDispatcherQueueHelper? wsqdHelper;
    private MicaController? micaController;
    private SystemBackdropConfiguration? configurationSource;

    public App() => this.InitializeComponent();

    /// <summary>
    /// Gets the Application object for the current application.
    /// </summary>
    /// <returns>The Application object for the current application.</returns>
    public static new App Current => Application.Current.As<App>();

    public MainWindow? StartupWindow { get; private set; }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        this.StartupWindow = new MainWindow();
        this.StartupWindow.SetAppTitleBar();
        this.StartupWindow.Activate();
        this.TrySetMicaBackdrop();
    }

    #region Mica

    private bool TrySetMicaBackdrop()
    {
        if (MicaController.IsSupported())
        {
            this.wsqdHelper = new WindowsSystemDispatcherQueueHelper();
            this.wsqdHelper.EnsureWindowsSystemDispatcherQueueController();

            this.configurationSource = new SystemBackdropConfiguration();
            this.StartupWindow!.Activated += this.Window_Activated;
            this.StartupWindow.Closed += this.Window_Closed;
            ((FrameworkElement)this.StartupWindow.Content).ActualThemeChanged += this.Window_ThemeChanged;

            this.configurationSource.IsInputActive = true;
            this.SetConfigurationSourceTheme();

            this.micaController = new MicaController();
            this.micaController.AddSystemBackdropTarget(this.StartupWindow.As<ICompositionSupportsSystemBackdrop>());
            this.micaController.SetSystemBackdropConfiguration(this.configurationSource);
            return true;
        }

        return false;
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (this.configurationSource is not null)
        {
            this.configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        }
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        if (this.micaController is not null)
        {
            this.micaController.Dispose();
            this.micaController = null;
        }

        this.StartupWindow!.Activated -= this.Window_Activated;
        this.configurationSource = null;
    }

    private void Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (this.configurationSource is not null)
        {
            this.SetConfigurationSourceTheme();
        }
    }

    private void SetConfigurationSourceTheme()
    {
        var actualTheme = ((FrameworkElement)this.StartupWindow!.Content).ActualTheme;
        this.configurationSource!.Theme = actualTheme switch
        {
            ElementTheme.Dark => SystemBackdropTheme.Dark,
            ElementTheme.Light => SystemBackdropTheme.Light,
            _ => SystemBackdropTheme.Default
        };
    }

    #endregion
}
