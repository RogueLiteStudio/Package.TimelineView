using UnityEditor;
using UnityEngine.UIElements;

namespace TimelineView
{
    public class CursorRect : ImmediateModeElement
    {
        protected override void ImmediateRepaint()
        {
            EditorGUIUtility.AddCursorRect(contentRect, MouseCursor.ResizeHorizontal);
        }
    }
}
