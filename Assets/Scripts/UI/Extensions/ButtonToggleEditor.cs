using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ButtonToggle))]
public class ButtonToggleEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        SerializedProperty group = serializedObject.FindProperty("group");
        EditorGUILayout.PropertyField(group);
        SerializedProperty highlight = serializedObject.FindProperty("highlight");
        EditorGUILayout.PropertyField(highlight);
        SerializedProperty isOn = serializedObject.FindProperty("isOn");
        EditorGUILayout.PropertyField(isOn);

        serializedObject.ApplyModifiedProperties();
    }
}