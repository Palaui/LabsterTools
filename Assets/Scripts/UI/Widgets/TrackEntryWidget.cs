using System;
using TMPro;
using UnityEngine;

public class TrackEntryWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private ButtonToggle buttonToggle;

    public EventHandler<TrackScriptable> Pressed;


    public void Set(TrackScriptable track, ButtonToggleGroup group)
    {
        title.text = track.name;
        buttonToggle.SetGroup(group);

        buttonToggle.onClick.AddListener(() => { Pressed(this, track); });
    }
}