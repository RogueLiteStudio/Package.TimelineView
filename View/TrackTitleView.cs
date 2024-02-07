using UnityEngine;
using UnityEngine.UIElements;

namespace TimelineView
{
    public class TrackTitleView : VisualElement
    {
        private VisualElement container;
        private Label title;
        private TrackView trackView;

        public string Title 
        {
            get => title.text;
            set => title.text = value;
        }

        public TrackTitleView(TrackView trackView)
        {
            this.trackView = trackView;
            trackView.RegisterCallback<GeometryChangedEvent>(OnTrackGeometryChangedEvent);
            var foldout = new Foldout();
            foldout.style.position = Position.Absolute;
            foldout.style.left = 0;
            foldout.style.top = 0;
            foldout.style.width = 20;
            foldout.style.height = 20;
            foldout.RegisterValueChangedCallback(evt => trackView.IsFold = evt.newValue);
            Add(foldout);
            container = new VisualElement();
            container.StretchToParentSize();
            container.style.left = 20;
            container.style.borderTopLeftRadius = 5;
            container.style.borderBottomLeftRadius = 5;
            container.style.borderLeftWidth = 5;
            container.style.borderLeftColor = Color.red;
            container.style.backgroundColor = TimelineViewStyle.TrackTitleColor;
            Add(container);

            title = new Label();
            title.style.unityTextAlign = TextAnchor.MiddleLeft;
            title.style.unityFontStyleAndWeight = FontStyle.Bold;
            title.StretchToParentSize();
            title.style.left = 5;
            title.style.height = TimelineViewStyle.TrackHeight;
            container.Add(title);

            style.minHeight = TimelineViewStyle.TrackHeight;
        }

        public void SetColor(Color color)
        {
            container.style.borderLeftColor = color;
        }

        private void OnTrackGeometryChangedEvent(GeometryChangedEvent evt)
        {
            style.height = evt.newRect.height;
        }
    }
}
