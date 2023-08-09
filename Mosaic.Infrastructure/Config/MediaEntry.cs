// ------------------------------------------------------------------------------
// <copyright file="MediaEntry.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config
{
    using System;
    using CsvHelper.Configuration.Attributes;

    public record MediaEntry(
        [property: Index(0)]
        Uri Mrl,

        [property: Index(1)]
        string? DisplayLabel = null);
}
