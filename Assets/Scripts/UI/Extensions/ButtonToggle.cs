using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonToggle : Button
{
    [SerializeField] internal ButtonToggleGroup group;
    [SerializeField] internal GameObject highlight;
    [SerializeField] internal bool isOn;


    private new void Awake()
    {
        base.Awake();
        if (group != null)
            group.Register(this);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        if (highlight != null)
            highlight.SetActive(isOn);
        if (group != null)
            group.Register(this);
    }
#endif

    internal void SetGroup(ButtonToggleGroup group)
    {
        this.group = group;
        group.Register(this);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isOn)
            return;
        if (group != null)
            group.OnButtonTogglePressed(this);
        isOn = true;

        base.OnPointerClick(eventData);
    }

    public void SetIsOn(bool value)
    {
        isOn = value;
        if (highlight != null)
            highlight.SetActive(isOn);

        if (value)
            onClick.Invoke();
    }

    public void SetIsOnWithoutNotify(bool value)
    {
        isOn = value;
        if (highlight != null)
            highlight.SetActive(isOn);
    }
}