using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeRowUI : MonoBehaviour
{
    [Header("데이터")]
    [SerializeField] private UpgradeDefinition _definition;

    [Header("UI")]
    [SerializeField] private Button _buyButton;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _costText;

    private void Awake()
    {
        if (_buyButton == null)
        {
            _buyButton = GetComponent<Button>();
        }
        _buyButton.onClick.AddListener(OnClickBuy);
    }

    private void OnEnable()
    {
        // 이벤트를 구독한다.
        if (PotionStock.Instance != null)
        {
            PotionStock.Instance.OnPotionChanged += HandlePotionChanged;
        }
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OnUpgradeLevelChanged += HandleUpgradeChanged;
        }
        Refresh();
    }

    private void OnDisable()
    {
        // 이벤트를 해제한다.
        if (PotionStock.Instance != null)
        {
            PotionStock.Instance.OnPotionChanged -= HandlePotionChanged;
        }
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OnUpgradeLevelChanged -= HandleUpgradeChanged;
        }
    }

    private void OnClickBuy()
    {
        if (_definition == null)
        {
            return;
        }
        UpgradeManager.Instance.TryBuy(_definition.UpgradeType);
        // Refresh를 즉시 반영 및 호출한다.
        Refresh();
    }

    private void HandlePotionChanged(double _)
    {
        Refresh();
    }

    private void HandleUpgradeChanged(EUpgradeType type, int _)
    {
        if (_definition != null && _definition.UpgradeType == type)
        {
            Refresh();
        }
    }

    private void Refresh()
    {
        if (_definition == null || UpgradeManager.Instance == null || PotionStock.Instance == null)
        {
            // 데이터가 없으면 버튼을 잠근다.
            if (_buyButton != null) _buyButton.interactable = false;
            return;
        }

        // 텍스트를 표기한다.
        if (_nameText != null) _nameText.text = string.IsNullOrEmpty(_definition.DisplayName)
            ? _definition.UpgradeType.ToString()
            : _definition.DisplayName;

        int level = UpgradeManager.Instance.GetLevel(_definition.UpgradeType);
        double cost = UpgradeManager.Instance.GetCost(_definition.UpgradeType);
        double potion = PotionStock.Instance.Potion;

        if (_levelText != null)
        {
            _levelText.text = $"Lv. {level}";
        }
        if (_costText != null)
        {
            _costText.text = $"{cost:0}";
        }

        // 구매 가능 여부를 체크한다.
        if (_buyButton != null)
        {
            _buyButton.interactable = (potion >= cost);
        }
    }
}
