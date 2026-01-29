using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
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
        _levelText.text = upgrade.Level.ToString("N1");
        _costText.text = upgrade.Cost.ToString();

        bool canLevelUp = UpgradeManager.Instance.CanLevelUp(upgrade.UpgradeDefinition.UpgradeType);

        _costText.color = canLevelUp ? Color.white : Color.red;
        _upgradeButton.interactable = canLevelUp;
    }

    public void LevelUp()
    {
        if (_upgrade == null)
        {
            return;
        }

        if (UpgradeManager.Instance.CanLevelUp(_upgrade.UpgradeDefinition.UpgradeType))
        {
            // todo. 이펙트, 애니메이션, 트위닝 등
        }
    }
}
