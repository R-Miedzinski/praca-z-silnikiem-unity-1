using System;
using Unity.Properties;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[AttributeUsage(AttributeTargets.Field)]
public class EffectIdSelectionAttribute : PropertyAttribute
{

}


[CustomPropertyDrawer(typeof(EffectIdSelectionAttribute))]
public class EffectIdSelectionDrawer : PropertyDrawer
{
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    EditorGUI.BeginProperty(position, label, property);

    List<string> options = new List<string> { "None" };
    List<string> ids = new List<string> { "" };

    foreach (var id in IdToEffectMap.GetAllEffectIds())
    {
      options.Add(id);
      ids.Add(id);
    }

    int index = ids.IndexOf(property.stringValue);
    if (index == -1) index = 0; // Default to "None" if not found
    // Draw the popup
    index = EditorGUI.Popup(position, label.text, index, options.ToArray());
    // Set the ID value
    property.stringValue = ids[index];
    EditorGUI.EndProperty();
  }
}