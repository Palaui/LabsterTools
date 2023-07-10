using UnityEngine.UIElements;

public abstract class BaseManager : VisualElement
{
    public abstract void Disable();
    public abstract void Initialize(VisualElement root);
}