using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrackPiecePlacer))]
public class TrackPiecePlacerEditor :Editor
{
    private TrackPiecePlacer script;


    private void OnEnable()
    {
        script = target as TrackPiecePlacer;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Snap To Grid"))
            script.SetOnGrid();

        if (GUILayout.Button("Add To Track"))
            script.AddToTrack();
    }
}