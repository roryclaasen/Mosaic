// ------------------------------------------------------------------------------
// <copyright file="IVideoPlayerTile.cs" company="Rory Claasen">
// Copyright (c) Rory Claasen. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------------

namespace Mosaic.Infrastructure
{
    using Mosaic.Infrastructure.Config;

    public interface IVideoPlayerTile
    {
        public string? Mrl { get; }

        public bool IsPlaying { get; }

        public double Progress { get; }

        public void SetProgress(long currentValue, long maxValue);

        public bool PlayVideo(MediaEntry entry);

        public void SetPause(bool pause);

        public void SetMute(bool mute);

        public void StopVideo();

        public void SetLabelVisibility(bool visible);
    }
}
