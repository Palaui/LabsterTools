using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarEntryWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image border;
    [SerializeField] private Image sprite;
    [SerializeField] private ButtonToggle buttonToggle;

    public EventHandler<CarScriptable> Pressed;


    public void Set(CarScriptable car, ButtonToggleGroup group)
    {
        title.text = car.name;
        border.color = car.Color;
        sprite.color = car.Color;

        buttonToggle.SetGroup(group);
        buttonToggle.onClick.AddListener(() => { Pressed(this, car); });
    }
}