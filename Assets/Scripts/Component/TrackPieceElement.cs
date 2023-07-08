using UnityEngine;

public class TrackPieceElement : MonoBehaviour
{
    private TrackScriptable track;
    private TrackPieceModel model;


    public void Initialize(TrackScriptable track, TrackPieceModel model)
    {
        this.track = track;
        this.model = model;
    }

    public void Modify()
    {
        if (track == null || model.piece == null)
        {
            Debug.LogError("TrackPieceElement: Track or model is null");
            return;
        }

        track.ModifyPiece(model);
    }

    public void Delete()
    {
        if (track == null || model.piece == null)
        {
            Debug.LogError("TrackPieceElement: Track or model is null");
            return;
        }

        track.RemovePiece(model);
        DestroyImmediate(gameObject);
    }
}