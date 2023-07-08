using UnityEditor;
using UnityEngine;

public static class UIExt
{
    public static Texture2D GetTrackPreview(string path, string id)
    {
        TrackPiece trackPiece = AssetDatabase.LoadAssetAtPath<TrackPiece>(AssetDatabase.GUIDToAssetPath(id));
        Editor editor = Editor.CreateEditor(trackPiece);
        Texture2D tex = editor.RenderStaticPreview(path, null, 50, 50);
        Object.DestroyImmediate(editor);
        return tex;
    }
}
