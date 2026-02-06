using Cysharp.Threading.Tasks;
using UnityEngine;

public class HybridAccountRepository : IAccountRepository
{
    private readonly LocalAccountRepository _local;
    private readonly FirebaseAccountRepository _firebase;

    private int _saveCounter = 0;

    public HybridAccountRepository()
    {
        _local = new LocalAccountRepository();
        _firebase = new FirebaseAccountRepository();
    }

    public bool IsEmailAvailable(string email)
    {
        return _local.IsEmailAvailable(email);
    }

    public async UniTask<AccountResult> Register(string email, string password)
    {
        var result = await _firebase.Register(email, password);
        if (result.Success)
        {
            await _local.Register(email, password);
        }
        return result;
    }

    public async UniTask<AccountResult> Login(string email, string password)
    {
        return await _firebase.Login(email, password);
    }

    public void Logout()
    {
        _firebase.Logout();
        _local.Logout();
    }

    public async UniTask Save(AccountSaveData data)
    {
        // 로컬에 저장한다.
        await _local.Save(data);
        _saveCounter++;

        // 5번 이후 그 뒤에 Firebase에 한 번 저장한다.
        if (_saveCounter >= 5)
        {
            await _firebase.Save(data);
            _saveCounter = 0;
        }
    }

    public async UniTask<AccountSaveData> Load()
    {
        var (localData, firebaseData) = await UniTask.WhenAll(_local.Load(), _firebase.Load());

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
