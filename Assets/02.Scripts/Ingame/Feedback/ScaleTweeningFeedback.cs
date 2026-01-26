using DG.Tweening;
using UnityEngine;

public class ScaleTweeningFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private ClickSlime _owner;

    private float _startPressX = 1.4f;
    private float _startPressY = 0.7f;
    private float _pressedDuration = 0.1f;
    private float _stayDuration = 0.04f;
    private float _endDuration = 0.24f;

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
        transform.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOScale(new Vector2(_startPressX, _startPressY), _pressedDuration).SetEase(Ease.OutQuad));
        sequence.AppendInterval(_stayDuration);
        sequence.Append(transform.DOScale(Vector2.one, _endDuration).SetEase(Ease.OutElastic));
    }

}
