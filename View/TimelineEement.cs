using UnityEngine.UIElements;

namespace TimelineView
{
    public class TimelineEement : VisualElement
    {
        private float scale = 1;
        public float Scale
        {
            get => scale;
            set
            {
                if (scale != value)
                {
                    scale = value;
                    OnScaleChanged();
                }
            }
        }
        protected virtual void OnScaleChanged()
        {
        }
    }
}
