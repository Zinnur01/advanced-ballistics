using Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace Editor.Attribute
{
    [CustomPropertyDrawer(typeof(DirectionAttribute))]
    public class DirectionDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                


                Vector2 vector = property.vector2Value;

                Rect leftButton = new Rect(position.x, position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(leftButton, vector.x == float.NegativeInfinity ? "-" : "-"))
                {
                    vector.x = vector.x == float.NegativeInfinity ? 0 : float.NegativeInfinity;
                }

                Rect leftValue = new Rect(position.x + EditorGUIUtility.singleLineHeight, position.y, position.center.x - (position.x + EditorGUIUtility.singleLineHeight) - 1, EditorGUIUtility.singleLineHeight);
                if (vector.x == float.NegativeInfinity)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.TextField(leftValue, "-∞");
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    vector.x = EditorGUI.FloatField(leftValue, vector.x);
                }

                Rect rightValue = new Rect(position.center.x + 1, position.y, position.center.x - (position.x + EditorGUIUtility.singleLineHeight) - 1, EditorGUIUtility.singleLineHeight);
                if (vector.y == float.PositiveInfinity)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUI.TextField(rightValue, "∞");
                    EditorGUI.EndDisabledGroup();
                }
                else
                {
                    vector.y = EditorGUI.FloatField(rightValue, vector.y);

                }

                Rect rightButton = new Rect(position.xMax - EditorGUIUtility.singleLineHeight, position.y, EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
                if (GUI.Button(rightButton, vector.y == float.PositiveInfinity ? "-" : "+"))
                {
                    vector.y = vector.y == float.PositiveInfinity ? 1 : float.PositiveInfinity;
                }

                property.vector2Value = vector;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 5f;
        }
    }
}