using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Track", menuName = "Labster/Track", order = 1)]
public class TrackScriptable : ScriptableObject
{
    [SerializeField] private List<TrackPieceModel> pieces;


    public IReadOnlyList<TrackPieceModel> Pieces => pieces;


#if UNITY_EDITOR

    public void AddPiece(TrackPieceModel piece)
    {
        pieces.Add(piece);
    }

    public void ModifyPiece(TrackPieceModel piece)
    {
        pieces.FirstOrDefault(entry => entry.piece.name == piece.piece.name);
    }

    public void RemovePiece(TrackPieceModel piece)
    {
        pieces.Remove(piece);
    }

#endif
}