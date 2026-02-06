using Firebase.Firestore;
using UnityEngine;

[FirestoreData]
public class AccountSaveData
{
    [FirestoreProperty]
    public string Email { get; set; }

    [FirestoreProperty]
    public string EncryptedPassword { get; set; }

    [FirestoreProperty]
    public long Timestamp { get; set; }  // 저장 시점

    public static AccountSaveData Default => new AccountSaveData()
    {
        Email = "",
        EncryptedPassword = "",
        Timestamp = 0
    };
}
