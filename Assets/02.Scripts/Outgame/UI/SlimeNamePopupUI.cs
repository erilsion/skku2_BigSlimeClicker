using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SlimeNamePopupUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup _group;       // 팝업 전체 (딤 포함이면 전체에)
    [SerializeField] private RectTransform _panel;     // 실제 패널
    [SerializeField] private TMP_InputField _input;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TMP_Text _errorText;      // 선택(없으면 null 가능)

    [Header("트윈")]
    [SerializeField] private float _openDuration = 0.22f;
    [SerializeField] private float _closeDuration = 0.16f;
    [SerializeField] private float _startScale = 0.86f;

    private Sequence _sequence;
    private bool _isOpen;

    private void Reset()
    {
        _group = GetComponentInChildren<CanvasGroup>();
        _panel = transform as RectTransform;
    }

    private void Awake()
    {
        if (_confirmButton != null)
        {
            _confirmButton.onClick.AddListener(Submit);
        }
        HideImmediate();
    }

    private void Update()
    {
        if (!_isOpen)
        {
            return;
        }

        // 엔터(리턴)로 확정한다.
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Submit();
        }
    }

    public void Open(string prefill = "")
    {
        _isOpen = true;

        gameObject.SetActive(true);
        if (_errorText != null)
        {
            _errorText.gameObject.SetActive(false);
        }

        if (_input != null)
        {
            _input.text = prefill ?? "";
            _input.ActivateInputField();
            _input.Select();
        }

        _sequence?.Kill(true);

        _group.alpha = 0f;
        _group.blocksRaycasts = true;
        _group.interactable = true;

        _panel.localScale = Vector3.one * _startScale;

        _sequence = DOTween.Sequence().SetUpdate(true);
        _sequence.Append(_group.DOFade(1f, _openDuration).SetEase(Ease.OutQuad));
        _sequence.Join(_panel.DOScale(1f, _openDuration).SetEase(Ease.OutBack));
    }

    public void Close()
    {
        if (!_isOpen)
        {
            return;
        }
        _isOpen = false;

        _sequence?.Kill(true);

        _group.blocksRaycasts = false;
        _group.interactable = false;

        _sequence = DOTween.Sequence().SetUpdate(true);
        _sequence.Append(_group.DOFade(0f, _closeDuration).SetEase(Ease.OutQuad));
        _sequence.Join(_panel.DOScale(_startScale, _closeDuration).SetEase(Ease.InQuad));
        _sequence.OnComplete(() => gameObject.SetActive(false));
    }

    private void HideImmediate()
    {
        _isOpen = false;

        if (_sequence != null)
        {
            _sequence.Kill(true);
        }

        if (_group != null)
        {
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;
        }

        if (_panel != null)
        {
            _panel.localScale = Vector3.one;
        }
        gameObject.SetActive(false);
    }

    private void Submit()
    {
        if (SlimeNameData.Instance == null)
        {
            ShowError("이름 저장소가 준비되지 않았어요...");
            return;
        }

        string name = _input != null ? _input.text : "";
        name = (name ?? "").Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            ShowError("이름을 입력해주세요! 6글자까지 할 수 있어요!");
            if (_input != null)
            {
                _input.ActivateInputField();
                _input.Select();
            }
            return;
        }

        // 길이 제한을 건다.
        if (name.Length > 6)
        {
            ShowError("슬라임이 기억하기엔 이름이 너무 길어요!");
            return;
        }

        SlimeNameData.Instance.SetName(name);
        Close();
    }

    private void ShowError(string msg)
    {
        if (_errorText == null)
        {
            return;
        }

        _errorText.text = msg;
        _errorText.gameObject.SetActive(true);

        // 에러 텍스트 살짝 튕기기 효과를 사용한다.
        _errorText.transform.DOKill(true);
        _errorText.transform.localScale = Vector3.one;
        _errorText.transform
            .DOPunchScale(new Vector3(0.08f, 0.08f, 0f), 0.18f, 8, 0.7f)
            .SetUpdate(true);
    }
}
