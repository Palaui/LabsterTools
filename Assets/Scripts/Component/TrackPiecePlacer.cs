using System;
using UnityEngine;

public class TrackPiecePlacer : MonoBehaviour
{
    private TrackScriptable track;
    private TrackPieceScriptable piece;


    public EventHandler Completed;
    public EventHandler<string> Alerted;


    public void Initialize(TrackScriptable track, TrackPieceScriptable piece)
    {
        this.track = track;
        this.piece = piece;
    }

    public void SetOnGrid()
    {
        Vector3 pos = transform.position / EditorConstants.GRID_SIZE;
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z)) * EditorConstants.GRID_SIZE;
        Vector3 euler = transform.eulerAngles / EditorConstants.RECT;
        euler.y = Mathf.Round(euler.y) * EditorConstants.RECT;
        transform.eulerAngles = euler;
    }

    public void AddToTrack()
    {
        if (!IsOnGrid())
        {
            Alerted?.Invoke(this, EditorConstants.ON_GRID_AUTO);
            SetOnGrid();
            return;
        }

        if (track == null || piece == null)
        {
            Debug.LogError("TrackPiecePlacer: Track or piece is null");
            return;
        }

        TrackPieceModel model = new TrackPieceModel(piece, transform.position, transform.rotation);
        track.AddPiece(model);

        Completed?.Invoke(this, EventArgs.Empty);
        Completed = null;
    }


    private bool IsOnGrid()
    {
        Vector3 pos = transform.position / EditorConstants.GRID_SIZE;
        return Mathf.Abs(pos.x - Mathf.Round(pos.x)) < EditorConstants.EPSILON &&
            Mathf.Abs(pos.y - Mathf.Round(pos.y)) < EditorConstants.EPSILON && 
            Mathf.Abs(pos.z - Mathf.Round(pos.z)) < EditorConstants.EPSILON;
    }

    private void OnDestroy()
    {
        Completed?.Invoke(this, EventArgs.Empty);
    }
}