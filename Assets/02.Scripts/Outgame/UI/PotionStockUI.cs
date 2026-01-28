using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class PotionStockUI : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] private TMP_Text _stockText;
    [SerializeField] private GameObject _stockTextParent;

    private bool _subscribed;
    private Coroutine _bindCoroutine;

    private float _startPressX = 1.4f;
    private float _startPressY = 1.2f;
    private float _pressedDuration = 0.12f;
    private float _endDuration = 0.12f;

    private void Start()
    {
        if (PotionStock.Instance == null)
        {
            return;
        }
        PotionStock.Instance.OnPotionChanged += OnStockChanged;

        // 켜질 때 현재 값도 바로 반영한다.
        OnStockChanged(PotionStock.Instance.Potion);
    }

    private void OnEnable()
    {
        // 이미 대기 중이면 중복 호출을 방지한다.
        if (_bindCoroutine != null) 
        {
            return;
        }
        _bindCoroutine = StartCoroutine(BindWhenReady_Coroutine());
    }

    private IEnumerator BindWhenReady_Coroutine()
    {
        while (PotionStock.Instance == null)
        {
            yield return null;
        }

        if (_subscribed)
        {
            yield break;
        }

        _subscribed = true;
        PotionStock.Instance.OnPotionChanged += OnStockChanged;
        OnStockChanged(PotionStock.Instance.Potion);

        // 여기까지 오면 역할 끝났으니 코루틴을 정리한다.
        _bindCoroutine = null;
    }

    private void OnDisable()
    {
        if (_bindCoroutine != null)
        {
            StopCoroutine(_bindCoroutine);
            _bindCoroutine = null;
        }

        if (_subscribed && PotionStock.Instance != null)
        {
            PotionStock.Instance.OnPotionChanged -= OnStockChanged;
        }

        _subscribed = false;
    }

    private void OnStockChanged(double potion)
    {
        if (_stockText == null)
        {
            return;
        }
        PlayTween();
        _stockText.text = potion.FormattedString();
    }

    // 최종 사용자 입장에서는 double은 그냥 숫자일 뿐인지 '재화'인지 모른다.
    // 규칙1. 재화는 0 미만일 수 없다. 그런데 지금은 음수가 가능해진다.
    // 규칙2. 재화를 표현할 때 무조건 FormattedString()을 써야 한다.
    public void PlayTween()
    {
        _stockTextParent.transform.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_stockTextParent.transform.DOScale(new Vector3(_startPressX, _startPressY, 1f), _pressedDuration).SetEase(Ease.OutQuad));
        sequence.Append(_stockTextParent.transform.DOScale(Vector3.one, _endDuration).SetEase(Ease.OutElastic));
    }
}
