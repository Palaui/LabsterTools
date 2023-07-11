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
        if (track == null || piece == null)
        {
            Alerted?.Invoke(this, EditorConstants.NO_TRACK_OR_PIECE_FOUND);
            return;
        }

        if (!transform.IsOnGrid())
        {
            Alerted?.Invoke(this, EditorConstants.ON_GRID_AUTO);
            SetOnGrid();
            return;
        }

        if (track.ContainsPieceAt(transform.position))
        {
            Alerted?.Invoke(this, EditorConstants.PIECE_ALREADY_IN_THAT_POSITION);
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