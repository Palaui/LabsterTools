using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Track", menuName = "Labster/Track", order = 1)]
public class TrackScriptable : ScriptableObject
{
    [SerializeField] private List<TrackPieceModel> pieceModels = new List<TrackPieceModel>();
    [SerializeField] private TrackPieceModel startPieceModel;
    [SerializeField] private TrackPieceModel endPieceModel;


    public IReadOnlyList<TrackPieceModel> PieceModels => pieceModels;
    public TrackPieceModel StartPieceModel => startPieceModel;
    public TrackPieceModel EndPieceModel => endPieceModel;


#if UNITY_EDITOR

    public void AddPiece(TrackPieceModel model)
    {
        if (!pieceModels.Any(entry => entry.id == model.id))
            pieceModels.Add(model);

        if (pieceModels.Count == 1)
        {
            startPieceModel = pieceModels[0];
            endPieceModel = pieceModels[0];
        }
    }

    public void ModifyPiece(TrackPieceModel model, TrackPieceFlag flag)
    {
        if (pieceModels.Any(entry => entry.id == model.id))
        {
            pieceModels.FirstOrDefault(entry => entry.id == model.id);

            switch (flag)
            {
                case TrackPieceFlag.Start: startPieceModel = model; break;
                case TrackPieceFlag.End: endPieceModel = model; break;
            }
        }
    }

    public void RemovePiece(TrackPieceModel model)
    {
        if (pieceModels.Any(entry => entry.id == model.id))
            pieceModels.Remove(model);
    }

#endif
}