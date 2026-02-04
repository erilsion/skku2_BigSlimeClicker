using Firebase.Firestore;
using System;
using UnityEngine;

[Serializable]
[FirestoreData]
public class UpgradeSaveData
{
    [FirestoreProperty]
    public double[] Upgrades { get; set; }

    public static UpgradeSaveData Default => new UpgradeSaveData()
    {
        Upgrades = new double[(int)EUpgradeType.Count]
    };
}
