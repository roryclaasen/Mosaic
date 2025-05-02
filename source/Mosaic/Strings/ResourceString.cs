// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Strings;

using Microsoft.UI.Xaml.Markup;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;

/// <summary>
/// Xaml extension to return a <see cref="string"/> value from resource file associated with a resource key.
/// </summary>
[MarkupExtensionReturnType(ReturnType = typeof(string))]
internal sealed partial class ResourceString : MarkupExtension
{
    private static readonly ResourceLoader ResourceLoader = ResourceLoader.GetForViewIndependentUse();

    /// <summary>
    /// Gets or sets associated ID from resource strings.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the associated language resource to use (ie: "en-US").
    /// Defaults to the OS language of current view.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Gets a string value from resource file associated with a resource key.
    /// </summary>
    /// <param name="name">Resource key name.</param>
    /// <returns>A string value from resource file associated with a resource key.</returns>
    public static string GetValue(string name) => ResourceLoader.GetString(name);

    /// <summary>
    /// Gets a string value from resource file associated with a resource key.
    /// </summary>
    /// <param name="name">Resource key name.</param>
    /// <param name="language">Optional language of the associated resource to use (ie: "en-US").
    /// Default is the OS language of current view.</param>
    /// <returns>A string value from resource file associated with a resource key.</returns>
    public static string GetValue(string name, string? language = null)
    {
        if (string.IsNullOrEmpty(language))
        {
            return GetValue(name);
        }

        var resourceContext = new ResourceContext() { Languages = [language] };
        var resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("Resources");
        return resourceMap.GetValue(name, resourceContext).ValueAsString;
    }

    /// <inheritdoc/>
    protected override object ProvideValue() => GetValue(this.Name, this.Language);
}
