using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Track", menuName = "Labster/Track", order = 1)]
public class TrackScriptable : ScriptableObject
{
    [SerializeField] private List<TrackPieceModel> pieceModels;


    public IReadOnlyList<TrackPieceModel> PieceModels => pieceModels;


#if UNITY_EDITOR

    public void AddPiece(TrackPieceModel model)
    {
        if (!pieceModels.Any(entry => entry.id == model.id))
            pieceModels.Add(model);
    }

    public void ModifyPiece(TrackPieceModel model)
    {
        if (pieceModels.Any(entry => entry.id == model.id))
            pieceModels.FirstOrDefault(entry => entry.id == model.id);
    }

    public void RemovePiece(TrackPieceModel model)
    {
        if (pieceModels.Any(entry => entry.id == model.id))
            pieceModels.Remove(model);
    }

#endif
}