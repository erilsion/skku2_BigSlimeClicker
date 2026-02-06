using Cysharp.Threading.Tasks;
using UnityEngine;

public class HybridAccountRepository
{
    private readonly LocalAccountRepository _local;
    private readonly FirebaseAccountRepository _firebase;

    private int _saveCounter = 0;

    public async UniTask Save(AccountSaveData data)
    {
        // 로컬에 저장한다.
        await _local.Save(data);
        _saveCounter++;

        // 5번 이후 그 뒤에 Firebase에 저장한다.
        if (_saveCounter > 5)
        {
            await _firebase.Save(data);
            _saveCounter = 0;
        }
    }

    public async UniTask<AccountSaveData> Load()
    {
        // 병렬로 둘 다 로드한다.
        var localTask = _local.Load();
        var firebaseTask = _firebase.Load();

        await UniTask.WhenAll(localTask, firebaseTask);

        var localData = await localTask;
        var firebaseData = await firebaseTask;

        // Timestamp 비교해서 최신 것을 반환한다.
        return GetLatestData(localData, firebaseData);
    }

    private AccountSaveData GetLatestData(AccountSaveData local, AccountSaveData firebase)
    {
        if (local == null)
        {
            return firebase;
        }
        if (firebase == null)
        {
            return local;
        }

        // Timestamp가 더 최신인 것 선택한다.
        return local.Timestamp > firebase.Timestamp ? local : firebase;
    }
}
