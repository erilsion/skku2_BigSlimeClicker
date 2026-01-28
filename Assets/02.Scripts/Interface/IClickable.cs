using UnityEngine;

public interface IClickable
{
    Vector2 Position { get; }
    bool OnClick(ClickInfo clickInfo);
}
