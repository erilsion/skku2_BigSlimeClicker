#if !UNITY_WEBGL || UNITY_EDITOR
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebaseCurrencyRepository : ICurrencyRepository
{
    private string Currency_Collection_Name = "Currency";
    private FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

    public async UniTaskVoid Save(CurrencySaveData saveData)
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            await _db.Collection(Currency_Collection_Name).Document(email).SetAsync(saveData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Currency 저장에 실패했습니다" + e.Message);
        }
    }

    public async UniTask<CurrencySaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            DocumentSnapshot snapshot = await _db.Collection(Currency_Collection_Name).Document(email).GetSnapshotAsync();
            CurrencySaveData data = snapshot.ConvertTo<CurrencySaveData>();
            if(data != null)
            {
                return data;
            }
            return CurrencySaveData.Default;
        }
        catch (Exception e)
        {
            Debug.LogError($"Currency 불러오기에 실패했습니다" + e.Message);
            return CurrencySaveData.Default;
        }
    }    
}
#endif
