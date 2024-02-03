using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

namespace TimelineView
{
    [System.Flags]
    public enum ClipDragCapabilities
    {
        None = 0,
        DragClip = 1,
        DragLeftHandle = 2,
        DragRightHandle = 4,
    }

    public class TimelineClipView : TimelineEement
    {
        private VisualElement colorbar;
        private VisualElement selectBackground;
        private VisualElement container;
        private Label titleLabel;
        private CursorRect leftHandle;
        private CursorRect rightHandle;
        private ClipDragCapabilities dragCapabilities;
        private ClipDragCapabilities dragType;
        public ClipDragCapabilities DragCapabilities
        {
            get => dragCapabilities;
            set 
            {
                dragCapabilities = value;
                leftHandle.visible = (value & ClipDragCapabilities.DragLeftHandle) != 0;
                rightHandle.visible = (value & ClipDragCapabilities.DragRightHandle) != 0;
            }
        }

        public string Title
        {
            get => titleLabel.text;
            set => titleLabel.text = value;
        }

        public int ChildCount => container.childCount;
        public IEnumerable<TimelineClipView> Clips=>container.Children().Select(it=>it as TimelineClipView);

        public TimelineClipView()
        {
            Init();
        }

        public void SetColor(Color color)
        {
            colorbar.style.backgroundColor = color;
        }

        private void Init()
        {
            style.minHeight = TimelineViewStyle.ClipHeight;

            style.borderLeftColor = Color.gray;
            style.borderRightColor = Color.gray;
            style.borderLeftWidth = 2;
            style.borderRightWidth = 2;

            colorbar = new VisualElement();
            colorbar.name = "ClipColorBar";
            colorbar.StretchToParentWidth();
            colorbar.style.height = 4;
            colorbar.style.bottom = 0;
            colorbar.style.backgroundColor = Color.blue;
            Add(colorbar);

            selectBackground = new VisualElement();
            selectBackground.name = "ClipSelectBackground";
            selectBackground.StretchToParentSize();
            selectBackground.visible = false;
            selectBackground.style.backgroundColor = TimelineViewStyle.ClipSelectColor;
            SetOutLine(selectBackground, Color.white, 1);
            Add(selectBackground);

            var contentBox = new VisualElement();
            contentBox.name = "ClipContentBox";
            contentBox.style.backgroundColor = TimelineViewStyle.ClipBackGroundColor;
            contentBox.style.height = TimelineViewStyle.ClipHeight;
            contentBox.style.flexDirection = FlexDirection.Row;
            contentBox.style.alignItems = Align.Center;
            contentBox.style.justifyContent = Justify.Center;
            contentBox.style.overflow = Overflow.Hidden;
            Add(contentBox);

            titleLabel = new Label();
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.style.color = Color.white;
            titleLabel.style.fontSize = 12;
            contentBox.Add(titleLabel);

            leftHandle = new CursorRect();
            leftHandle.name = "ClipLeftHandle";
            leftHandle.style.left = 0;
            leftHandle.style.width = 3;
            leftHandle.style.top = 0;
            leftHandle.style.bottom = 0;
            leftHandle.RegisterCallback<MouseDownEvent>(OnClickLeftHandle);
            Add(leftHandle);

            rightHandle = new CursorRect();
            rightHandle.name = "ClipRightHandle";
            rightHandle.style.width = TimelineViewStyle.ClipHandleWidth;
            rightHandle.style.right = 0;
            rightHandle.style.width = 3;
            rightHandle.style.top = 0;
            rightHandle.style.bottom = 0;
            rightHandle.RegisterCallback<MouseDownEvent>(OnClickRightHandle);
            Add(rightHandle);

            container = new VisualElement();
            container.name = "ClipContainer";
            container.style.marginTop = TimelineViewStyle.ClipHeight;
            Add(container);

            this.AddManipulator(new DragManipulator(OnDragStart, OnDragMove, OnDragEnd));
        }

        public void AddChildClip(TimelineClipView clip)
        {
            container.Add(clip);
        }

        private void OnClickLeftHandle(MouseDownEvent evt)
        {
            if ((dragCapabilities & ClipDragCapabilities.DragLeftHandle) == 0)
                return;
            dragType = ClipDragCapabilities.DragLeftHandle;
        }

        private void OnClickRightHandle(MouseDownEvent evt)
        {
            if ((dragCapabilities & ClipDragCapabilities.DragRightHandle) == 0)
                return;
            dragType = ClipDragCapabilities.DragRightHandle;
        }

        private void OnDragStart(DragEventData data)
        {
            if (dragCapabilities == ClipDragCapabilities.None)
                return;
            switch (dragType)
            {
                case ClipDragCapabilities.DragClip:
            	    break;
                case ClipDragCapabilities.DragLeftHandle:
                    break;
                case ClipDragCapabilities.DragRightHandle:
                    break;
            }
        }

        private void OnDragMove(DragEventData data)
        {
            if (dragCapabilities == ClipDragCapabilities.None)
                return;
            switch (dragType)
            {
                case ClipDragCapabilities.DragClip:
                    break;
                case ClipDragCapabilities.DragLeftHandle:
                    break;
                case ClipDragCapabilities.DragRightHandle:
                    break;
            }
        }
        private void OnDragEnd(DragEventData data)
        {
            dragType = dragCapabilities & ClipDragCapabilities.DragClip;
        }


        public static void SetOutLine(VisualElement element, Color color, int width)
        {
            element.style.borderBottomColor = color;
            element.style.borderBottomWidth = width;

            element.style.borderTopColor = color;
            element.style.borderTopWidth = width;

            element.style.borderLeftColor = color;
            element.style.borderLeftWidth = width;

            element.style.borderRightColor = color;
            element.style.borderRightWidth = width;
        }
    }
}
