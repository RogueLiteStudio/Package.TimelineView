using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace TimelineView
{
    public class TrackGroupView : TimelineEement
    {
        private VisualElement container;

        public int TrackCount => container.childCount;

        public IEnumerable<TrackView> Tracks => container.Children().Select(it => it as TrackView);

        public TrackGroupView()
        {
            container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;
            container.StretchToParentSize();
            Add(container);
        }

        public void AddTrack(TrackView trackView)
        {
            container.Add(trackView);
        }
    }
}
