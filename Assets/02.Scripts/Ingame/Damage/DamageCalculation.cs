using System;
using UnityEngine;

public class DamageCalculation : MonoBehaviour
{
    public static DamageCalculation Instance { get; private set; }

    private double _manualBase;
    private double _autoBase;

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

    public void Initialize(double manualBase, double autoBase)
    {
        _manualBase = manualBase;
        _autoBase = autoBase;
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
        ManualBonus = 0;
        AutoBonus = 0;

        foreach (var upgrade in UpgradeManager.Instance.GetAll())
        {
            switch (upgrade.UpgradeDefinition.UpgradeType)
            {
                case EUpgradeType.ManualSmall:
                case EUpgradeType.ManualMedium:
                case EUpgradeType.ManualLarge:
                    ManualBonus += upgrade.Bonus;
                    break;

                case EUpgradeType.AutoSmall:
                case EUpgradeType.AutoMedium:
                case EUpgradeType.AutoLarge:
                    AutoBonus += upgrade.Bonus;
                    break;
            }
        }
        OnBonusChanged?.Invoke();
    }
}
