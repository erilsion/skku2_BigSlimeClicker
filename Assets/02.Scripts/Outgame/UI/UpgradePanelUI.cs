using UnityEngine;
using System.Collections.Generic;

public class UpgradePanelUI : MonoBehaviour
{
    [SerializeField] private List<UpgradeButtonUI> _upgradeButtonsUI;

    private readonly Dictionary<EUpgradeType, UpgradeButtonUI> _uiMap = new();

    private void Awake()
    {
        _uiMap.Clear();
        foreach (var ui in _upgradeButtonsUI)
        {
            if (ui == null) continue;
            _uiMap[ui.Type] = ui;
        }
    }

    private void OnEnable()
    {
        CurrencyManager.OnDataChanged += Refresh;
        UpgradeManager.OnDataChanged += Refresh;
    }

    private void OnDisable()
    {
        CurrencyManager.OnDataChanged -= Refresh;
        UpgradeManager.OnDataChanged -= Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        if (UpgradeManager.Instance == null) return;

        foreach (var upgrade in UpgradeManager.Instance.GetAll())
        {
            if (_uiMap.TryGetValue(upgrade.UpgradeDefinition.UpgradeType, out var ui))
            {
                ui.Refresh(upgrade);
            }
        }
    }
}
