using UnityEngine;

public class CameraShakeFeedback : MonoBehaviour, IFeedback
{
    [Header("기준 재화")]
    [SerializeField] private double _maxPotion = 1_000_000;

    [Header("체감 곡선")]
    [SerializeField] private float _powerPow = 0.6f;

    public void Play(ClickInfo clickInfo)
    {
        PlayCameraShake();
    }

    private void PlayCameraShake()
    {
        if (PotionStock.Instance == null)
        {
            return;
        }
        double potion = PotionStock.Instance.Potion;

        float t = Mathf.Clamp01((float)(potion / _maxPotion));
        t = Mathf.Pow(t, _powerPow); // Earth / Scale 연출과 통일

        CameraShake.Instance.Shake(t);
    }
}
