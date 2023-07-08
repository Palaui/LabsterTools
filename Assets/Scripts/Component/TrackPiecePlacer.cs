using UnityEngine;

public class TrackPiecePlacer : MonoBehaviour
{
    private const float GRID_SIZE = 6;
    private const float RECT = 90;

    private TrackScriptable track;
    private TrackPieceScriptable piece;


    public void Initialize(TrackScriptable track, TrackPieceScriptable piece)
    {
        this.track = track;
        this.piece = piece;
    }

    public void SetOnGrid()
    {
        Vector3 pos = transform.position / GRID_SIZE;
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z)) * GRID_SIZE;
        Vector3 euler = transform.eulerAngles / RECT;
        euler.y = Mathf.Round(euler.y) * RECT;
    }

    public void AddToTrack()
    {
        if (track == null || piece == null)
        {
            Debug.LogError("TrackPiecePlacer: Track or piece is null");
            return;
        }

        TrackPieceModel model = new TrackPieceModel();
        model.piece = piece;
        model.position = transform.position;
        model.rotation = transform.rotation;
        track.AddPiece(model);
    }
}