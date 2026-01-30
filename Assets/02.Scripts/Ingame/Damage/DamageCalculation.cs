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

    public double GetManualDamage() => _manualBase + ManualBonus;
    public double GetAutoDamage() => _autoBase + AutoBonus;

    public void UpgradeManual(double plus)
    {
        ManualBonus += plus;
        OnBonusChanged?.Invoke();
    }

    public void UpgradeAuto(double plus)
    {
        AutoBonus += plus;
        OnBonusChanged?.Invoke();
    }
}
