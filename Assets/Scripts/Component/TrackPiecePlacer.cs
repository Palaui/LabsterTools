#if UNITY_EDITOR

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
        transform.SetOnGrid();
    }

    public void Cancel()
    {
        Completed?.Invoke(this, EventArgs.Empty);
        Completed = null;
    }

    public void AddToTrack()
    {
        if (!transform.IsOnGrid())
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

    private void OnDestroy()
    {
        Completed?.Invoke(this, EventArgs.Empty);
    }
}


#endif