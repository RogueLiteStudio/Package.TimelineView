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

    public class ClipView : TimelineEement
    {
        private VisualElement colorbar;
        private VisualElement selectBackground;
        private VisualElement container;
        private Label titleLabel;
        private CursorRect leftHandle;
        private CursorRect rightHandle;
        private ClipDragCapabilities dragCapabilities;
        private ClipDragCapabilities dragType;
        private int startFrame;
        private int frameLength = 1;

        public int StartFrame
        {
            get => startFrame;
            set
            {
                startFrame = value;
                style.left = startFrame * TimelineViewStyle.FrameWidth * Scale;
            }
        }

        public int FrameLength
        {
            get => frameLength;
            set
            {
                frameLength = value;
                style.width = frameLength * TimelineViewStyle.FrameWidth * Scale;
            }
        }

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
        public IEnumerable<ClipView> Clips=>container.Children().Select(it=>it as ClipView);

        public ClipView()
        {
            Init();
            OnScaleChanged();
        }

        public void SetColor(Color color)
        {
            colorbar.style.backgroundColor = color;
        }

        protected override void OnScaleChanged()
        {
            style.left = startFrame * TimelineViewStyle.FrameWidth * Scale;
            style.width = frameLength * TimelineViewStyle.FrameWidth * Scale;
            foreach (var child in Clips)
            {
                child.Scale = Scale;
            }
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

            this.AddManipulator(new DragManipulator(OnDragEvent));
        }

        public void AddChildClip(ClipView clip)
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

        private void OnDragEvent(DragEventData evt)
        {
            if (dragCapabilities == ClipDragCapabilities.None)
                return;
            if (evt.State == DragStateType.End)
            {
                dragType = dragCapabilities & ClipDragCapabilities.DragClip;
                return;
            }
            if (evt.State == DragStateType.Start)
            {
                OnDragStart();
            }
            int frameCount = Mathf.FloorToInt(evt.Delta.x / (TimelineViewStyle.FrameWidth * Scale));
            frameCount = HandleDragableFrameCount(dragType, frameCount);
            if (frameCount != 0)
            {
                switch (dragType)
                {
                    case ClipDragCapabilities.DragClip:
                        StartFrame += frameCount;
                        FrameLength += frameCount;
                        break;
                    case ClipDragCapabilities.DragLeftHandle:
                        StartFrame += frameCount;
                        break;
                    case ClipDragCapabilities.DragRightHandle:
                        FrameLength += frameCount;
                        break;
                }
                OnFrameInfoChanged();
            }
        }

        protected virtual void OnDragStart()
        {
            //子类注册Undo
        }

        protected virtual int HandleDragableFrameCount(ClipDragCapabilities type, int frameCount)
        {
            //子类检查是否可以拖动
            return frameCount;
        }

        protected virtual void OnFrameInfoChanged()
        {
            //更新记录的帧相关信息
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
