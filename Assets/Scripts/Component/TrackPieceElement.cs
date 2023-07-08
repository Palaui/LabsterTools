using System;
using UnityEngine;

public class TrackPieceElement : MonoBehaviour
{
    private TrackScriptable track;
    private TrackPieceModel model;


    public EventHandler<string> Alerted;


    public void Initialize(TrackScriptable track, TrackPieceModel model)
    {
        this.track = track;
        this.model = model;
    }

    public void SetOnGrid()
    {
        Vector3 pos = transform.position / EditorConstants.GRID_SIZE;
        transform.position = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z)) * EditorConstants.GRID_SIZE;
        Vector3 euler = transform.eulerAngles / EditorConstants.RECT;
        euler.y = Mathf.Round(euler.y) * EditorConstants.RECT;
        transform.eulerAngles = euler;
    }

    public void Modify()
    {
        if (!IsOnGrid())
        {
            Alerted?.Invoke(this, EditorConstants.ON_GRID_AUTO);
            SetOnGrid();
            return;
        }

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


    private bool IsOnGrid()
    {
        Vector3 pos = transform.position / EditorConstants.GRID_SIZE;
        return Mathf.Abs(pos.x - Mathf.Round(pos.x)) < EditorConstants.EPSILON && 
            Mathf.Abs(pos.y - Mathf.Round(pos.y)) < EditorConstants.EPSILON && 
            Mathf.Abs(pos.z - Mathf.Round(pos.z)) < EditorConstants.EPSILON;
    }
}