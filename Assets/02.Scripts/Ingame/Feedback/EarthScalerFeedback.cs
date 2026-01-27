using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EarthScalerFeedback : MonoBehaviour
{
    [Header("대상")]
    [SerializeField] private Transform _target;

    [Header("기준 재화")]
    [SerializeField] private double _maxPotion = 1_000_000;

    [Header("스케일")]
    [SerializeField] private float _maxScale = 1f;
    [SerializeField] private float _minScale = 0.001f;

    private float _scalePow = 0.6f;

    [Header("X축 이동")]
    [SerializeField] private float _startX = 16.6f;
    [SerializeField] private float _endX = 4.8f;

    private float _xCurvePower = 1.4f;

    [Header("트윈")]
    [SerializeField] private float _tweenDuration = 0.3f;

    private float _shakeDuration = 0.15f;
    private float _shakeX = 0.12f;
    private float _shakeY = 0.08f;
    private int _shakeVibration = 12;
    private float _shakeRandomDegree = 90f;

    private Tween _tween;
    private Coroutine _bindCoroutine;

    private void Awake()
    {
        if (_target == null)
        {
            _target = transform;
        }
    }

    private void OnEnable()
    {
        if (_bindCoroutine != null)
        {
            return;
        }
        _bindCoroutine = StartCoroutine(Bind_Coroutine());
    }

    private IEnumerator Bind_Coroutine()
    {
        while (PotionStock.Instance == null)
        {
            yield return null;
        }
        PotionStock.Instance.OnPotionChanged += OnPotionChanged;
        OnPotionChanged(PotionStock.Instance.Potion);
        _bindCoroutine = null;
    }

    private void OnDisable()
    {
        if (_bindCoroutine != null)
        {
            StopCoroutine(_bindCoroutine);
            _bindCoroutine = null;
        }
        if (PotionStock.Instance != null)
        {
            PotionStock.Instance.OnPotionChanged -= OnPotionChanged;
        }
        _tween?.Kill();
    }

    private void OnPotionChanged(double potion)
    {
        float t = Mathf.Clamp01((float)(potion / _maxPotion));

        // 초반에 줄어드는 게 티나도록 체감을 조절한다.
        t = Mathf.Pow(t, _scalePow);

        float scale = Mathf.Lerp(_maxScale, _minScale, t);
        float xT = 1f - Mathf.Pow(1f - t, _xCurvePower);
        float xPosition = Mathf.Lerp(_startX, _endX, xT);

        _tween?.Kill();

        Vector3 targetPosition = _target.position;
        targetPosition.x = xPosition;

        _tween = DOTween.Sequence()
            .Append(_target.DOScale(scale, _tweenDuration))
            .Join(_target.DOMoveX(xPosition, _tweenDuration))
            .Join(
                _target.DOShakePosition(
                    _shakeDuration,
                    new Vector3(_shakeX, _shakeY, 0f),
                    _shakeVibration,
                    _shakeRandomDegree,
                    false,
                    true
                )
            );
    }
}
