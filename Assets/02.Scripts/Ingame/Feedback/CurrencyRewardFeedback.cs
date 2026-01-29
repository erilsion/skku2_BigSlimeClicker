using UnityEngine;

public class CurrencyRewardFeedback : MonoBehaviour, IFeedback
{
    public void Play(ClickInfo clickInfo)
    {
        CurrencyManager.Instance.Add(ECurrencyType.Potion, (Currency)clickInfo.Damage);
    }
}
