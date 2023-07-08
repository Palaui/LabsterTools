using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackPiecePlacer))]
public class TrackPiecePlacerEditor :Editor
{
    private TrackPiecePlacer script;
    private string alert = "";


    private void OnEnable()
    {
        script = target as TrackPiecePlacer;
        script.Alerted += (_, str) => alert = str;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Snap To Grid"))
            script.SetOnGrid();

        if (GUILayout.Button("Cancel"))
            script.Cancel();

        if (GUILayout.Button("Add To Track"))
            script.AddToTrack();

        if (alert != "")
            EditorGUILayout.HelpBox(alert, MessageType.Warning);
    }
}