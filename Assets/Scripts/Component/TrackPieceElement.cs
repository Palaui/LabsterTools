#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

public class TrackPieceElement : MonoBehaviour
{
    [SerializeField] private TrackPieceFlag flag = TrackPieceFlag.None;

    private TrackScriptable track;
    private TrackPieceModel model;
    private string id;


    public string Id => id;


    public EventHandler Modified;
    public EventHandler<string> Alerted;


    public void Initialize(TrackScriptable track, TrackPieceModel model)
    {
        this.track = track;
        this.model = model;
        id = model.id;
    }

    public void SetOnGrid()
    {
        transform.SetOnGrid();
    }

    public void Modify()
    {
        if (!transform.IsOnGrid())
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

        track.ModifyPiece(model, flag);
        Modified?.Invoke(this, EventArgs.Empty);
        Alerted?.Invoke(this, "");
        Selection.objects = new UnityEngine.Object[] { };

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

#endif