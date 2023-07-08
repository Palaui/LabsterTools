using UnityEngine;

[CreateAssetMenu(fileName = "TrackPiece", menuName = "Labster/TrackPiece", order = 1)]
public class TrackPieceScriptable : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite icon;
    

    public GameObject Prefab => prefab;
    public Sprite Icon => icon;

    public bool IsCorrectlySet => prefab != null && icon != null;
}