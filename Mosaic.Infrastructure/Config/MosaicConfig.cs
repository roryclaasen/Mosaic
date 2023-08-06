// ------------------------------------------------------------------------------
// <copyright file="MosaicConfig.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure.Config
{
    using System;
    using System.Collections.Generic;

    public record MosaicConfig(IEnumerable<MediaEntry> Sources);

    public record MediaEntry(Uri Mrl, string? DisplayLabel = null);
}
