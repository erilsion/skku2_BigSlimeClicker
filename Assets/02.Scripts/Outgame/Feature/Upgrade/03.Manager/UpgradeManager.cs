using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public static event Action OnDataChanged;

    [SerializeField] private UpgradeDefinitionTableSO _definitionTable;

    private Dictionary<EUpgradeType, Upgrade> _upgrades = new();
    public Upgrade Get(EUpgradeType type) => _upgrades[type];
    public List<Upgrade> GetAll() => _upgrades.Values.ToList();

    private IUpgradeRepository _repository;

    private void Awake()
    {
        Instance = this;

        _repository = new LocalUpgradeRepository();
        var saveData = _repository.Load();

        _upgrades.Clear();
        foreach (var definition in _definitionTable.Definitions)
        {
            if (definition == null || _definitionTable.Definitions == null)
            {
                throw new Exception("UpgradeDefinitionTableSO가 비어있습니다: {definition.UpgradeType}");
            }
            if (_upgrades.ContainsKey(definition.UpgradeType))
            {
                throw new Exception($"중복된 업그레이드 정의입니다: {definition.UpgradeType}");
            }
            var upgrade = new Upgrade(definition);

            int index = (int)definition.UpgradeType;
            upgrade.RestoreLevel((int)saveData.Upgrades[index]);

            _upgrades.Add(definition.UpgradeType, upgrade);
        }

        OnDataChanged?.Invoke();
    }

    public bool CanLevelUp(EUpgradeType type)
    {
        if (!_upgrades.TryGetValue(type, out Upgrade upgrade))
        {
            return false;
        }
        if (upgrade.IsMaxLevel)
        {
            return false;
        }
        return CurrencyManager.Instance.CanAfford(ECurrencyType.Potion, upgrade.Cost);
    }

    public bool TryLevelUp(EUpgradeType type)
    {
        if(!_upgrades.TryGetValue(type, out Upgrade upgrade)) 
        {
            return false; 
        }

        Currency cost = upgrade.Cost;

        if(!CurrencyManager.Instance.TrySpend(ECurrencyType.Potion, upgrade.Cost))
        {
            return false;
        }

        if (!upgrade.TryLevelUp())
        {
            return false;
        }

        OnDataChanged?.Invoke();
        SaveToRepository();
        return true;
    }

    private void SaveToRepository()
    {
        var data = UpgradeSaveData.Default;

        foreach (var pair in _upgrades)
        {
            data.Upgrades[(int)pair.Key] = pair.Value.Level;
        }

        _repository.Save(data);
    }
}
