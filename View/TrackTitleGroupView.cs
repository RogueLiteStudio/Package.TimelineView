using UnityEngine.UIElements;

namespace TimelineView
{
    public class TrackTitleGroupView : VisualElement
    {
        private VisualElement container;
        public TrackTitleGroupView()
        {
            container = new VisualElement();
            container.style.flexDirection = FlexDirection.Column;
            container.StretchToParentSize();
            Add(container);
        }

        public void AddTrackTitle(TrackTitleView trackTitleView)
        {
            container.Add(trackTitleView);
        }
    }
}
