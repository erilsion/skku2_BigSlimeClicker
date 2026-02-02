using System;
using UnityEngine;

[Serializable]
public class UpgradeSaveData
{
    public double[] Upgrades;

    public static UpgradeSaveData Default => new UpgradeSaveData()
    {
        Upgrades = new double[(int)EUpgradeType.Count]
    };
}
