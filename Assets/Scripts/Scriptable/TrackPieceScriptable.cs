using UnityEngine;

[CreateAssetMenu(fileName = "TrackPiece", menuName = "Labster/TrackPiece", order = 1)]
public class TrackPieceScriptable : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite icon;
    

    public GameObject Prefab
    {
        get => prefab;
        set { if (Application.isEditor) prefab = value; }
    }
    public Sprite Icon
    {
        get => icon;
        set { if (Application.isEditor) icon = value; }
    }

    public bool IsCorrectlySet => prefab != null && icon != null;
}