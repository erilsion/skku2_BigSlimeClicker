using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{

private void Awake()
    {
        if (_upgradeButton != null)
        {
            _upgradeButton.onClick.AddListener(LevelUp);
        }
    }

    [Header("텍스트")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _bonusText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _costText;

    [Header("버튼")]
    [SerializeField] private Button _upgradeButton;

    [Header("타입")]
    [SerializeField] private EUpgradeType _type;
    public EUpgradeType Type => _type;

    private Upgrade _upgrade;

    public void Refresh(Upgrade upgrade)
    {
        _upgrade = upgrade;

        _nameText.text = upgrade.UpgradeDefinition.DisplayName.ToString();
        _levelText.text = "Lv: " + upgrade.Level.ToString("N0");
        _costText.text = "Cost: " + upgrade.Cost.ToString();

        bool canLevelUp = UpgradeManager.Instance.CanLevelUp(upgrade.UpgradeDefinition.UpgradeType);

        _costText.color = canLevelUp ? Color.white : Color.red;
        _upgradeButton.interactable = canLevelUp;
    }

public void LevelUp()
    {
        if (_upgrade == null)
        {
            Debug.LogWarning("Upgrade is null! Button not initialized.");
            return;
        }

        bool success = UpgradeManager.Instance.TryLevelUp(_upgrade.UpgradeDefinition.UpgradeType);
        if (success)
        {
            // todo: 성공 이펙트/사운드
        }
        else
        {
            // todo: 실패 피드백(재화 부족/최대 레벨)
        }
    }
}
