using UnityEngine;
using System;
using System.Collections.Generic;

public class PastUpgradeManager : MonoBehaviour
{
    public static PastUpgradeManager Instance { get; private set; }

    [SerializeField] private List<UpgradeDefinition> _definitions = new();

    private readonly Dictionary<EUpgradeType, UpgradeDefinition> _defMap = new();
    private readonly Dictionary<EUpgradeType, int> _levels = new();
    private readonly Dictionary<EUpgradeType, double> _bonuses = new();

    public event Action<EUpgradeType, int> OnUpgradeLevelChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _defMap.Clear();
        foreach (var def in _definitions)
        {
            if (def == null)
            {
                continue;
            }

            // 중복 방지
            _defMap[def.UpgradeType] = def;

            if (!_levels.ContainsKey(def.UpgradeType))
            {
                _levels[def.UpgradeType] = 0;
            }
        }
    }

    public int GetLevel(EUpgradeType type) => _levels.TryGetValue(type, out var lv) ? lv : 0;

    public double GetBonus(EUpgradeType type) => _bonuses.TryGetValue(type, out var bonus) ? bonus : 0;

    public double GetCost(EUpgradeType type)
    {
        if (!_defMap.TryGetValue(type, out var def))
        {
            return double.PositiveInfinity;
        }

        int lv = GetLevel(type);
        return def.BaseCost * Math.Pow(def.CostGrowth, lv);
    }

    public bool TryBuy(EUpgradeType type)
    {
        if (!_defMap.TryGetValue(type, out var def))
        {
            return false;
        }

        double cost = GetCost(type);
        if (!PotionStock.Instance.TrySpend(cost))
        {
            return false;
        }

        int newLv = GetLevel(type) + 1;
        _levels[type] = newLv;

        Apply(def);

        OnUpgradeLevelChanged?.Invoke(type, newLv);
        return true;
    }

    private void Apply(UpgradeDefinition def)
    {
        // 단순 + 누적을 계산한다.
        if (def.TargetClickType == EClickType.Manual)
        {
            DamageCalculation.Instance.UpgradeManual(def.PlusPerLevel);
        }
        else
        {
            DamageCalculation.Instance.UpgradeAuto(def.PlusPerLevel);
        }
    }

    public UpgradeDefinition GetDefinition(EUpgradeType type) => _defMap.TryGetValue(type, out var def) ? def : null;
}
