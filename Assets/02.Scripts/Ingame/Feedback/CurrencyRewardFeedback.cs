using UnityEngine;

public class CurrencyRewardFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        PotionStock.Instance.Add(clickInfo.Damage);
    }
}
