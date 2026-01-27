using UnityEngine;

public class SlimeClickable : MonoBehaviour, IClickable
{
    public Vector2 Position => transform.position;

    private void OnEnable()
    {
        ClickableRegistry.Register(this);
    }

    private void OnDisable()
    {
        ClickableRegistry.Unregister(this);
    }

    public bool OnClick(ClickInfo clickInfo)
    {
        return true;
    }
}
