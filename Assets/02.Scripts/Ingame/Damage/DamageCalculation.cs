using System;
using UnityEngine;

public class DamageCalculation : MonoBehaviour
{
    public static DamageCalculation Instance { get; private set; }

    [SerializeField] private double _manualBase = 10f;
    [SerializeField] private double _autoBase = 0f;

    // 업그레이드 시 사용하는 정보이다.
    public double ManualBonus { get; private set; }
    public double AutoBonus { get; private set; }

    public event Action OnBonusChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        UpgradeManager.OnDataChanged += RecalculateBonus;
    }

    private void OnDisable()
    {
        UpgradeManager.OnDataChanged -= RecalculateBonus;
    }

    public double GetManualDamage() => _manualBase + ManualBonus;
    public double GetAutoDamage() => _autoBase + AutoBonus;

private void RecalculateBonus()
    {
        if (UpgradeManager.Instance == null)
        {
            return;
        }
        
        ManualBonus = 0;
        AutoBonus = 0;

        foreach (var upgrade in UpgradeManager.Instance.GetAll())
        {
            if (upgrade.UpgradeDefinition.TargetClickType == EClickType.Manual)
            {
                ManualBonus += upgrade.Bonus;
            }
            else
            {
                AutoBonus += upgrade.Bonus;
            }
        }
        OnBonusChanged?.Invoke();
    }
}
