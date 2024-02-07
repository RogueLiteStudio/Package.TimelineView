using UnityEngine;
using UnityEngine.UIElements;

namespace TimelineView
{
    public struct DragEventData
    {
        public Vector2 StartMousePosition;
        public Vector2 CuurentMousePosition;
        public Vector2 Delta;
        public DragStateType State;
    }

    public enum DragStateType
    {
        Start,
        Move,
        End
    }

    public class DragManipulator : MouseManipulator
    {
        private DragStateType dragState;
        private Vector2 mouseDownPosition;

        private readonly System.Action<DragEventData> OnDragEvent;

        public DragManipulator(System.Action<DragEventData> onDragEvent)
        {
            OnDragEvent = onDragEvent;
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
                dragState = DragStateType.Start;
                mouseDownPosition = evt.localMousePosition;
                target.CaptureMouse();
                evt.StopPropagation();
            }
        }

        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (dragState != DragStateType.End)
            {
                var ed = new DragEventData
                {
                    StartMousePosition = mouseDownPosition,
                    CuurentMousePosition = evt.localMousePosition,
                    Delta = evt.mouseDelta,
                    State = dragState
                };
                dragState = DragStateType.Move;
                OnDragEvent?.Invoke(ed);
            }

        }

        private void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button == 0)
            {
                if (dragState == DragStateType.Move)
                {
                    dragState = DragStateType.End;
                    var ed = new DragEventData
                    {
                        StartMousePosition = mouseDownPosition,
                        CuurentMousePosition = evt.localMousePosition,
                        Delta = evt.mouseDelta,
                        State = DragStateType.End
                    };
                    OnDragEvent?.Invoke(ed);
                }
                target.ReleaseMouse();
            }
        }
    }
}
