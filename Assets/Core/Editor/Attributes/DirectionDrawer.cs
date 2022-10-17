using Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.Attributes
{
    [CustomPropertyDrawer(typeof(DirectionAttribute))]
    public class DirectionDrawer : PropertyDrawer
    {
        private DirectionAttribute directionAttribute;

        private bool dragging = false;

        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, GUIContent label)
        {
            if (directionAttribute == null)
            {
                directionAttribute = attribute as DirectionAttribute;
            }

            Event e = Event.current;

            EditorGUI.BeginDisabledGroup(true);
            UnityEngine.Vector2 vector = EditorGUI.Vector2Field(position, label, property.vector2Value);
            EditorGUI.EndDisabledGroup();

            position = new UnityEngine.Rect(position.x, position.y + EditorGUI.GetPropertyHeight(property, label) + EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight * 6f);

            float circleSize = EditorGUIUtility.singleLineHeight * 6f;
            float radius = circleSize / 2;
            UnityEngine.Rect centerPosition = new UnityEngine.Rect(position.center.x - radius, position.y, circleSize, circleSize);

            UnityEngine.Vector2 mousePosition = (e.mousePosition - centerPosition.center) / radius;

            vector.y *= -1;

            if (!dragging && e.type == EventType.MouseDown)
            {
                if (mousePosition.magnitude <= 1f)
                {
                    dragging = true;
                }
            }

            if (dragging && e.type == EventType.MouseUp)
            {
                dragging = false;
            }

            if (dragging && (e.type == EventType.MouseDrag || e.type == EventType.MouseDown))
            {
                vector = mousePosition;

                if (directionAttribute.normalized || vector.magnitude > 1f)
                {
                    vector.Normalize();
                }

                HandleUtility.Repaint();
            }


            GUIUtility.RotateAroundPivot(Mathf.Atan2(-vector.x, vector.y) * Mathf.Rad2Deg, centerPosition.center);
            EditorGUI.DrawRect(new UnityEngine.Rect(centerPosition.center, new UnityEngine.Vector2(1, vector.magnitude * radius)), Color.white);
            GUI.matrix = Matrix4x4.identity;

            UnityEngine.Vector2 rectPosition = centerPosition.center + vector * radius - new UnityEngine.Vector2(0.05f, 0.05f) * radius;
            EditorGUI.DrawRect(new UnityEngine.Rect(rectPosition, new UnityEngine.Vector2(0.1f, 0.1f) * radius), Color.red);

            vector.y *= -1;

            property.vector2Value = vector;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label) + EditorGUIUtility.standardVerticalSpacing * 2f + EditorGUIUtility.singleLineHeight * 6f;
        }
    }
}