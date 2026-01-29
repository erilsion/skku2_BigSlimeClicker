using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }
    public static event Action OnDataChanged;

    [SerializeField] private UpgradeSpecTableSO _specTable;

    private Dictionary<EUpgradeType, Upgrade> _upgrades = new();

    private void Awake()
    {
        Instance = this;

        foreach(var specData in _specTable.UpgradeSpecDatas)
        {
            if (_upgrades.ContainsKey(specData.Type))
            {
                throw new Exception($"다음 스펙 데이터가 없습니다 {specData.Type}");
            }
            _upgrades.Add(specData.Type, new Upgrade(specData));
        }
        OnDataChanged.Invoke();
    }
}
