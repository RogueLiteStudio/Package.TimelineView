using UnityEngine;

namespace TimelineView
{
    public static class TimelineViewStyle
    {
        public const float ClipHeight = 30;
        public const float ClipColorbarHeight = 4;
        public const float ClipHandleWidth = 5;
        public static readonly Color ClipBackGroundColor = new Color(70 / 255f, 70 / 255f, 70 / 255f, 1f);
        public static readonly Color ClipSelectColor = new Color(1, 1, 1, 0.3f);

        public const float TrackInterval = 5;
        public const float TrackHeight = 32;
        public static readonly Color TrackBackgroundColor = new Color(0f, 0f, 1f, 0.5f);
    }
}
