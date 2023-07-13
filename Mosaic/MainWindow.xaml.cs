// ------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic
{
    using System;
    using System.Linq;
    using Microsoft.UI.Windowing;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media.Animation;
    using Microsoft.UI.Xaml.Navigation;

    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.NavigationViewControl.SelectedItem = this.NavigationViewControl.MenuItems.OfType<NavigationViewItem>().First();
            this.ContentFrame.Navigate(typeof(Views.HomePage), null, new EntranceNavigationTransitionInfo());
        }

        public void SetAppTitleBar()
        {
            this.Title = this.GetAppTitleFromSystem();
            if (AppWindowTitleBar.IsCustomizationSupported() && false)
            {
                this.ExtendsContentIntoTitleBar = true;
                this.SetTitleBar(this.AppTitleBar);
            }
            else
            {
                this.AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        public string GetAppTitleFromSystem()
        {
            return Windows.ApplicationModel.Package.Current.DisplayName;
        }

        private void NavigationViewControl_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                this.ContentFrame.Navigate(typeof(Views.SettingsPage), null, args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer is not null && args.InvokedItemContainer.Tag is not null)
            {
                Type newPage = Type.GetType(args.InvokedItemContainer.Tag.ToString());
                this.ContentFrame.Navigate(newPage, null, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void NavigationViewControl_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (this.ContentFrame.CanGoBack)
            {
                this.ContentFrame.GoBack();
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            this.NavigationViewControl.IsBackEnabled = this.ContentFrame.CanGoBack;

            if (this.ContentFrame.SourcePageType == typeof(Views.SettingsPage))
            {
                this.NavigationViewControl.SelectedItem = (NavigationViewItem)this.NavigationViewControl.SettingsItem;
            }
            else if (this.ContentFrame.SourcePageType is not null)
            {
                this.NavigationViewControl.SelectedItem = this.NavigationViewControl.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(this.ContentFrame.SourcePageType.FullName.ToString()));
            }

            this.NavigationViewControl.Header = ((NavigationViewItem)this.NavigationViewControl.SelectedItem)?.Content?.ToString();
        }
    }
}
