using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebaseUpgradeRepository : IUpgradeRepository
{
    private string Upgrade_Collection_Name = "Upgrade";
    private FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

    public async UniTaskVoid Save(UpgradeSaveData saveData)
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            await _db.Collection(Upgrade_Collection_Name).Document(email).SetAsync(saveData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Upgrade 저장에 실패했습니다" + e.Message);
        }
    }

    public async UniTask<UpgradeSaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            DocumentSnapshot snapshot = await _db.Collection(Upgrade_Collection_Name).Document(email).GetSnapshotAsync();
            UpgradeSaveData data = snapshot.ConvertTo<UpgradeSaveData>();
            if (data != null)
            {
                return data;
            }
            return UpgradeSaveData.Default;
        }
        catch (Exception e)
        {
            Debug.LogError($"Upgrade 불러오기에 실패했습니다" + e.Message);
            return UpgradeSaveData.Default;
        }
    }
}
