using DG.Tweening;
using UnityEngine;

public class ScaleTweeningFeedback : MonoBehaviour
{
    [SerializeField] private ClickSlime _owner;

    private float _endScaleValue = 1.3f;
    private float _scaleduration = 0.4f;

    private void Awake()
    {
        _owner = GetComponent<ClickSlime>();
    }

    // 역할: 스케일 트위닝 피드백에 대한 로직 담당
    public void PlayTween()
    {
        _owner.transform.DOKill();
        _owner.transform.DOScale(_endScaleValue, _scaleduration).OnComplete(() =>
        {
            _owner.transform.localScale = Vector3.one;
        });
    }

}
