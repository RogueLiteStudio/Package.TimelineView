using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
namespace TimelineView
{
    public class TimelineTrackView : TimelineEement
    {
        private VisualElement container;
        private bool isFold;
        public bool IsFold 
        {
            get => isFold;
            set
            {
                isFold = value;
                container.style.display = isFold ? DisplayStyle.None : DisplayStyle.Flex;
            }
        }

        public int ClipCount => container.childCount;

        public IEnumerable<TimelineClipView> Clips => container.Children().Select(it => it as TimelineClipView);

        public TimelineTrackView()
        {
            container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;
            container.style.top = 1;
            container.style.bottom = 1;
            container.style.left = 0;
            container.style.right = 0;
            Add(container);
            style.minHeight = TimelineViewStyle.TrackHeight;
            style.backgroundColor = TimelineViewStyle.TrackBackgroundColor;
        }


        public void AddClip(TimelineClipView clipView)
        {
            container.Add(clipView);
        }
    }
}