using System;
using UnityEngine;

public class DamageCalculation : MonoBehaviour
{
    public static DamageCalculation Instance { get; private set; }

    [Header("기본 재화 획득량")]
    [SerializeField] private double _manualBase = 10;
    [SerializeField] private double _autoBase = 3;

    // 업그레이드 시 사용하는 정보이다.
    public double ManualBonus { get; private set; }
    public double AutoBonus { get; private set; }

    public event Action OnBonusChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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
