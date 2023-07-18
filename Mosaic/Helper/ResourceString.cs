// ------------------------------------------------------------------------------
// <copyright file="ResourceString.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Helper
{
    using Microsoft.UI.Xaml.Markup;
    using Windows.ApplicationModel.Resources;

    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    internal sealed class ResourceString : MarkupExtension
    {
#if MICROSOFT_WINDOWSAPPSDK_SELFCONTAINED
        private static readonly ResourceLoader CurrentResourceLoader = ResourceLoader.GetForViewIndependentUse();
#else
        private static readonly ResourceLoader CurrentResourceLoader = ResourceLoader.GetForCurrentView();
#endif

        public string Name { get; set; }

        protected override object ProvideValue()
            => CurrentResourceLoader.GetString(this.Name);
    }
}
