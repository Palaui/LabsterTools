using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackPieceElement))]
public class TrackPieceElementEditor : Editor
{
    private TrackPieceElement script;


    private void OnEnable()
    {
        script = target as TrackPieceElement;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Modify"))
            script.Modify();

        if (GUILayout.Button("Delete"))
            script.Delete();
    }
}