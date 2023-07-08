using UnityEditor;
using UnityEngine;

public static class UIExt
{
    public static Texture2D GetTrackPreview(string path, string id)
    {
        TrackPieceScriptable trackPiece = AssetDatabase.LoadAssetAtPath<TrackPieceScriptable>(AssetDatabase.GUIDToAssetPath(id));
        Editor editor = Editor.CreateEditor(trackPiece);
        Texture2D tex = editor.RenderStaticPreview(path, null, 50, 50);
        Object.DestroyImmediate(editor);
        return tex;
    }
}
