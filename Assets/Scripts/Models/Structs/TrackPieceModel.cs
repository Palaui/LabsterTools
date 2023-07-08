using System;
using UnityEngine;

[Serializable]
public struct TrackPieceModel
{
    public string id;

    public TrackPieceScriptable piece;
    public Vector3 position;
    public Quaternion rotation;


    public TrackPieceModel(TrackPieceScriptable piece, Vector3 position, Quaternion rotation)
    {
        id = Guid.NewGuid().ToString();

        this.piece = piece;
        this.position = position;
        this.rotation = rotation;
    }
}