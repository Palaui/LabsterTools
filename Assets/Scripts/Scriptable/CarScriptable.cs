using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Labster/Car", order = 1)]
public class CarScriptable : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Color color;
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float grip;
    [SerializeField] private float riskAcceleration;


    public GameObject Prefab => prefab;

    public Color Color
    {
        get => color;
        set { if (Application.isEditor) color = value; }
    }
    public float Acceleration
    {
        get => acceleration;
        set { if (Application.isEditor) acceleration = value; }
    }
    public float MaxSpeed
    {
        get => maxSpeed;
        set { if (Application.isEditor) maxSpeed = value; }
    }
    public float TurnSpeed
    {
        get => turnSpeed;
        set { if (Application.isEditor) turnSpeed = value; }
    }
    public float Grip
    {
        get => grip;
        set { if (Application.isEditor) grip = value; }
    }
    public float RiskAcceleration
    {
        get => riskAcceleration;
        set { if (Application.isEditor) riskAcceleration = value; }
    }
}