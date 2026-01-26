using DG.Tweening;
using UnityEngine;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private ClickSlime _owner;

    private float _endScaleValue = 1.3f;
    private float _scaleDuration = 0.14f;

    private void Awake()
    {
        _owner = GetComponent<ClickSlime>();
    }

    public void Play(ClickInfo clickInfo)
    {
        PlayTween();
    }

    // 역할: 스케일 트위닝 피드백에 대한 로직 담당
    public void PlayTween()
    {
        _owner.transform.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_owner.transform.DOScale(_endScaleValue, _scaleDuration).SetEase(Ease.OutBack));
        sequence.Append(_owner.transform.DOScale(1f, _scaleDuration).SetEase(Ease.InOutQuad));
    }

}
