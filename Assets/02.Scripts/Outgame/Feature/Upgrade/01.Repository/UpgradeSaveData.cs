using UnityEngine;

public class UpgradeSaveData : MonoBehaviour
{
    public double[] Upgrades;

    public static UpgradeSaveData Default => new UpgradeSaveData()
    {
        Upgrades = new double[(int)EUpgradeType.Count]
    };
}
