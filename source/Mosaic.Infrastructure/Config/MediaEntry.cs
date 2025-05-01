// Copyright (c) Rory Claasen. All rights reserved.
// Licensed under the MIT license. See LICENSE in the project root for license information.

namespace Mosaic.Infrastructure.Config;

using System;
using CsvHelper.Configuration.Attributes;

public record MediaEntry(
    [property: Index(0)]
    Uri Mrl,

    [property: Index(1)]
    string? DisplayLabel = null);
