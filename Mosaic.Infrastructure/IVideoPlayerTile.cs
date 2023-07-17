// ------------------------------------------------------------------------------
// <copyright file="IVideoPlayerTile.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using System;

    public interface IVideoPlayerTile
    {
        public string? Mrl { get; }

        public bool IsPlaying { get; }

        public bool PlayVideo(Uri mrl, string? label = null);

        public void SetPause(bool pause);

        public void StopVideo();

        public void SetLabelVisibility(bool visible);
    }
}
