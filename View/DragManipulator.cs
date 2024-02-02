using UnityEngine;
using UnityEngine.UIElements;

namespace TimelineView
{
    public struct DragEventData
    {
        public Vector2 LocalMousePosition;
        public Vector2 Delta;
    }

    public class DragManipulator : MouseManipulator
    {
        private bool gotMouseDown;
        private bool isDragStart;
        private Vector2 mouseDownPosition;

        private readonly System.Action<DragEventData> OnDragStart;
        private readonly System.Action<DragEventData> OnDragMove;
        private readonly System.Action<DragEventData> OnDragEnd;

        public DragManipulator(System.Action<DragEventData> onDragStart, System.Action<DragEventData> onDragMove, System.Action<DragEventData> onDragEnd)
        {
            OnDragStart = onDragStart;
            OnDragMove = onDragMove;
            OnDragEnd = onDragEnd;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<MouseDownEvent>(OnMouseDown);
            target.RegisterCallback<MouseMoveEvent>(OnMouseMove);
            target.RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<MouseDownEvent>(OnMouseDown);
            target.UnregisterCallback<MouseMoveEvent>(OnMouseMove);
            target.UnregisterCallback<MouseUpEvent>(OnMouseUp);
        }

        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.button == 0)
            {
                isDragStart = false;
                gotMouseDown = true;
                mouseDownPosition = evt.localMousePosition;
                target.CaptureMouse();
                evt.StopPropagation();
            }
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (gotMouseDown && evt.pressedButtons == 1)
            {
                var ed = new DragEventData 
                {
                    LocalMousePosition = mouseDownPosition,
                };
                OnDragStart?.Invoke(ed);
                gotMouseDown = false;
                isDragStart = true;
            }

            if (isDragStart)
            {
                var ed = new DragEventData
                {
                    LocalMousePosition = evt.localMousePosition,
                    Delta = evt.mouseDelta
                };
                OnDragMove?.Invoke(ed);
            }

        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button == 0)
            {
                if (gotMouseDown)
                {
                    gotMouseDown = false;
                }

                if (isDragStart)
                {
                    isDragStart = false;
                    var ed = new DragEventData
                    {
                        LocalMousePosition = evt.localMousePosition,
                        Delta = evt.mouseDelta
                    };
                    OnDragEnd?.Invoke(ed);
                }

                target.ReleaseMouse();
            }
        }
    }
}
