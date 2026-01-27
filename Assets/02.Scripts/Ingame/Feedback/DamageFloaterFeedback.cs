using UnityEngine;

public class DamageFloaterFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        DamageFloaterSpawner.Instance.ShowDamage(clickInfo);
    }
}
