using System;
using UnityEngine;

public class TrackPiecePlacer : MonoBehaviour
{
    private const string ON_GRID_AUTO = "Track piece was not on grid and has been placed automatically. Please, ask again to add to track if the current position is correct.";

    private const float GRID_SIZE = 6;
    private const float RECT = 90;
    private const float EPSILON = 0.01f;

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
        Vector3 pos = transform.position / GRID_SIZE;
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z)) * GRID_SIZE;
        Vector3 euler = transform.eulerAngles / RECT;
        euler.y = Mathf.Round(euler.y) * RECT;
        transform.eulerAngles = euler;
    }

    public void AddToTrack()
    {
        if (!IsOnGrid())
        {
            Alerted?.Invoke(this, ON_GRID_AUTO);
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
        Vector3 pos = transform.position / GRID_SIZE;
        return Mathf.Abs(pos.x - Mathf.Round(pos.x)) < EPSILON && Mathf.Abs(pos.y - Mathf.Round(pos.y)) < EPSILON && Mathf.Abs(pos.z - Mathf.Round(pos.z)) < EPSILON;
    }

    private void OnDestroy()
    {
        Completed?.Invoke(this, EventArgs.Empty);
    }
}