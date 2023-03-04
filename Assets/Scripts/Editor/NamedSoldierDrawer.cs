using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(NamedSoldierAttribute))]
public class NamedSoldierDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        try
        {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                EditorGUI.ObjectField(rect, property, new GUIContent(((NamedSoldierAttribute)attribute).names[pos]));
            }
            else
            {
                EditorGUI.TextField(rect, label, property.stringValue);
            }
        }
        catch
        {
            EditorGUI.ObjectField(rect, property, label);
        }
    }
}