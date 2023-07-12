// ------------------------------------------------------------------------------
// <copyright file="WindowHelper.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.UI;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Media;
    using WinRT.Interop;

    public class WindowHelper
    {
        private static List<Window> _activeWindows = new List<Window>();

        public static IReadOnlyList<Window> ActiveWindows { get { return _activeWindows; } }

        public static Window CreateWindow()
        {
            var newWindow = new Window
            {
                SystemBackdrop = new MicaBackdrop()
            };
            TrackWindow(newWindow);
            return newWindow;
        }

        public static void TrackWindow(Window window)
        {
            window.Closed += (sender, args) =>
            {
                _activeWindows.Remove(window);
            };
            _activeWindows.Add(window);
        }

        public static AppWindow GetAppWindow(Window window)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(window);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        public static Window GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot != null)
            {
                foreach (Window window in _activeWindows)
                {
                    if (element.XamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }

            return null;
        }
    }
}
