using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarDataWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;

    [SerializeField] private Slider acceleration;
    [SerializeField] private Slider maxSpeed;
    [SerializeField] private Slider turnSpeed;
    [SerializeField] private Slider grip;
    [SerializeField] private Slider risk;


    public void Set(CarScriptable car)
    {
        if (car == null)
        {
            title.text = "No car selected";
            acceleration.value = acceleration.minValue;
            maxSpeed.value = maxSpeed.minValue;
            turnSpeed.value = turnSpeed.minValue;
            grip.value = grip.minValue;
            risk.value = risk.minValue;
            return;
        }

        title.text = car.name;
        acceleration.value = car.Acceleration;
        maxSpeed.value = car.MaxSpeed;
        turnSpeed.value = car.TurnSpeed;
        grip.value = car.Grip;
        risk.value = car.RiskAcceleration;
    }
}