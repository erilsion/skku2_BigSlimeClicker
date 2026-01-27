using UnityEngine;
using System;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Serializable]
    public class UpgradeConfig
    {
        public EClickType id;
        public double baseCost = 10;
        public double costGrowth = 1.15;   // 레벨당 가격 증가 배수
        public double plusPerLevel = 1;    // 레벨당 보너스 증가량(+=)
    }

    [SerializeField] private UpgradeConfig[] _configs;

    private readonly Dictionary<EClickType, UpgradeConfig> _configMap = new();
    private readonly Dictionary<EClickType, int> _levels = new();

    public event Action<EClickType, int> OnUpgradeLevelChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _configMap.Clear();
        foreach (var c in _configs)
        {
            if (c == null) continue;
            _configMap[c.id] = c;
            if (!_levels.ContainsKey(c.id)) _levels[c.id] = 0;
        }
    }

    public int GetLevel(EClickType id) => _levels.TryGetValue(id, out var lv) ? lv : 0;

    public double GetCost(EClickType id)
    {
        if (!_configMap.TryGetValue(id, out var c)) return double.PositiveInfinity;
        int lv = GetLevel(id);

        // baseCost * growth^lv
        return c.baseCost * Math.Pow(c.costGrowth, lv);
    }

    public bool TryBuy(EClickType id)
    {
        if (!_configMap.TryGetValue(id, out var c)) return false;

        double cost = GetCost(id);
        if (!PotionStock.Instance.TrySpend(cost)) return false;

        _levels[id] = GetLevel(id) + 1;

        if (id == EClickType.Manual) DamageCalculation.Instance.UpgradeManual(c.plusPerLevel);
        else if (id == EClickType.Auto) DamageCalculation.Instance.UpgradeAuto(c.plusPerLevel);

        OnUpgradeLevelChanged?.Invoke(id, _levels[id]);
        return true;
    }
}
