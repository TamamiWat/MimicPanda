using System;

namespace Unity.LiveCapture.VirtualCamera
{
    /// <summary>
    /// The virtual camera data that can be recorded in a take and played back.
    /// </summary>
    [Flags]
    enum VirtualCameraChannelFlags : int
    {
        [Description("No channel recorded.")]
        None = 0,

        [Description("Record position.")]
        Position = 1 << 0,

        [Description("Record rotation.")]
        Rotation = 1 << 1,

        [Description("Record focal length.")]
        FocalLength = 1 << 2,

        [Description("Record focus distance.")]
        FocusDistance = 1 << 3,

        [Description("Record aperture.")]
        Aperture = 1 << 4,

        [Description("Record all supported channels.")]
        All = ~0
    }
}
