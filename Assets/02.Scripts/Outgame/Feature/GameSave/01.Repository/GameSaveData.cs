using Firebase.Firestore;
using UnityEngine;

public class GameSaveData : MonoBehaviour
{
    [FirestoreProperty] public long Timestamp { get; set; }
    [FirestoreProperty] public CurrencySaveData Currency { get; set; }
    [FirestoreProperty] public UpgradeSaveData Upgrade { get; set; }

    public static GameSaveData Default => new GameSaveData
    { 
        Timestamp = 0,
        Currency = CurrencySaveData.Default,
        Upgrade = UpgradeSaveData.Default
    };
}
