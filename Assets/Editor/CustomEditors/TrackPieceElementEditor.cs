using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackPieceElement))]
public class TrackPieceElementEditor : Editor
{
    private TrackPieceElement script;
    private string alert = "";


    private void OnEnable()
    {
        script = target as TrackPieceElement;
        script.Alerted += (_, str) => alert = str;
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.LabelField("ID", script.Id);

        if (GUILayout.Button("Modify"))
            script.Modify();

        if (GUILayout.Button("Delete"))
            script.Delete();

        if (alert != "")
            EditorGUILayout.HelpBox(alert, MessageType.Warning);
    }
}