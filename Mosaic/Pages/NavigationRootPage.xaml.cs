using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Mosaic.Helper;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mosaic.Pages
{
    public sealed partial class NavigationRootPage : Page
    {
        public NavigationRootPage()
        {
            this.InitializeComponent();

            this.Loaded += (object sender, RoutedEventArgs e) =>
            {
                var window = WindowHelper.GetWindowForElement(sender as UIElement);
                window.Title = this.AppTitleText;
                window.ExtendsContentIntoTitleBar = true;
                window.SetTitleBar(this.AppTitleBar);
            };
        }

        public string AppTitleText
        {
            get
            {
#if DEBUG
                return "Mosaic Video Viewer Dev";
#else
                return "Mosaic Video Viewer";
#endif
            }
        }

        public void Navigate(Type pageType, object targetPageArguments = null, NavigationTransitionInfo navigationTransitionInfo = null)
        {
            var args = new NavigationRootPageArgs(this, targetPageArguments);
            this.rootFrame.Navigate(pageType, args, navigationTransitionInfo);
        }
    }

    public record NavigationRootPageArgs(NavigationRootPage NavigationRootPage, object Parameter);
}
