using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebaseGameSaveRepository : IGameSaveRepository
{
    private readonly FirebaseAuth _auth = FirebaseAuth.DefaultInstance;
    private readonly FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;

    private DocumentReference _documentReference => _db.Collection("GameSave").Document(_auth.CurrentUser.UserId);

    public async UniTask Save(GameSaveData data)
    {
        try
        {
            if (_auth.CurrentUser == null)
            {
                return;
            }
            await _documentReference.SetAsync(data);
        }
        catch (Exception e)
        {
            Debug.LogError($"파이어베이스 데이터 저장 실패: {e.Message}");
        }
    }

    public async UniTask<GameSaveData> Load()
    {
        try
        {
            if (_auth.CurrentUser == null)
            {
                return GameSaveData.Default;
            }

            var snap = await _documentReference.GetSnapshotAsync();
            if (!snap.Exists)
            {
                return GameSaveData.Default;
            }
            var data = snap.ConvertTo<GameSaveData>();
            return data ?? GameSaveData.Default;
        }
        catch (Exception e)
        {
            Debug.LogError($"파이어베이스 데이터 로드 실패: {e.Message}");
            return GameSaveData.Default;
        }
    }
}
