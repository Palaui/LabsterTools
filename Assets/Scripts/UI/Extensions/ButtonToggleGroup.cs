using System.Collections.Generic;
using UnityEngine;

public class ButtonToggleGroup : MonoBehaviour
{
    private List<ButtonToggle> buttonToggles = new List<ButtonToggle>();

    internal void Register(ButtonToggle buttonToggle)
    {
        if (buttonToggles.Contains(buttonToggle))
            return;

        buttonToggles.Add(buttonToggle);
    }

    internal void Unregister(ButtonToggle buttonToggle)
    {
        if (!buttonToggles.Contains(buttonToggle))
            return;

        buttonToggles.Remove(buttonToggle);
    }

    internal void OnButtonTogglePressed(ButtonToggle buttonToggle)
    {
        foreach (ButtonToggle entry in buttonToggles)
            entry.SetIsOnWithoutNotify(entry == buttonToggle);
    }
}