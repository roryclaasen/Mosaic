// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic;

using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Mosaic.Util;
using WinRT;

public partial class App
{
    private WindowsSystemDispatcherQueueHelper? wsqdHelper;
    private MicaController? micaController;
    private SystemBackdropConfiguration? configurationSource;

    private bool TrySetMicaBackdrop()
    {
        if (this.Window is not null && MicaController.IsSupported())
        {
            this.wsqdHelper = new WindowsSystemDispatcherQueueHelper();
            this.wsqdHelper.EnsureWindowsSystemDispatcherQueueController();

            this.configurationSource = new SystemBackdropConfiguration();
            this.Window.Activated += this.Mica_Window_Activated;
            this.Window.Closed += this.Mica_Window_Closed;

            if (this.Window.Content is FrameworkElement element)
            {
                element.ActualThemeChanged += this.Mica_Window_ThemeChanged;
            }

            this.configurationSource.IsInputActive = true;
            this.SetConfigurationSourceTheme();

            this.micaController = new MicaController();
            this.micaController.AddSystemBackdropTarget(this.Window.As<ICompositionSupportsSystemBackdrop>());
            this.micaController.SetSystemBackdropConfiguration(this.configurationSource);

            return true;
        }

        return false;
    }

    private void Mica_Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (this.configurationSource is not null)
        {
            this.configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        }
    }

    private void Mica_Window_Closed(object sender, WindowEventArgs args)
    {
        if (this.micaController is not null)
        {
            this.micaController.Dispose();
            this.micaController = null;
        }

        if (this.Window is not null)
        {
            this.Window.Activated -= this.Mica_Window_Activated;
        }

        this.configurationSource = null;
    }

    private void Mica_Window_ThemeChanged(FrameworkElement sender, object args)
    {
        if (this.configurationSource is not null)
        {
            this.SetConfigurationSourceTheme();
        }
    }

    private void SetConfigurationSourceTheme()
    {
        if (this.Window?.Content is FrameworkElement element)
        {
            var actualTheme = element.ActualTheme;
            this.configurationSource!.Theme = actualTheme switch
            {
                ElementTheme.Dark => SystemBackdropTheme.Dark,
                ElementTheme.Light => SystemBackdropTheme.Light,
                _ => SystemBackdropTheme.Default
            };
        }
    }
}
