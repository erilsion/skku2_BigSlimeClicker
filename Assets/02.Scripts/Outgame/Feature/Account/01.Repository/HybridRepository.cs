using Cysharp.Threading.Tasks;
using UnityEngine;

public class HybridRepository : IAccountRepository, IGameSaveRepository
{
    private readonly LocalAccountRepository _localAccount;
    private readonly LocalGameSaveRepository _localGameSave;
#if !UNITY_WEBGL || UNITY_EDITOR
    private readonly FirebaseAccountRepository _firebaseAccount;
    private readonly FirebaseGameSaveRepository _firebaseGameSave;
#endif

    private int _saveCounter = 0;

    public HybridRepository()
    {
        _localAccount = new LocalAccountRepository();
        _localGameSave = new LocalGameSaveRepository();
#if !UNITY_WEBGL || UNITY_EDITOR
        _firebaseAccount = new FirebaseAccountRepository();
        _firebaseGameSave = new FirebaseGameSaveRepository();
#endif
    }

    public bool IsEmailAvailable(string email)
    {
        return _localAccount.IsEmailAvailable(email);
    }

    public async UniTask<AccountResult> Register(string email, string password)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        var result = await _firebaseAccount.Register(email, password);
        if (result.Success)
        {
            await _localAccount.Register(email, password);
        }
        return result;
#else
        {
            var result = await _localAccount.Register(email, password);
            return result;
        }
#endif
    }

    public async UniTask<AccountResult> Login(string email, string password)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return await _firebaseAccount.Login(email, password);
        return await _localAccount.Login(email, password);
#else
        return await _localAccount.Login(email, password);
#endif
    }

    public void Logout()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        _firebaseAccount.Logout();
        _localAccount.Logout();
#else
        _localAccount.Logout();
#endif
    }

    public async UniTask Save(GameSaveData data)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        // 로컬에 저장한다.
        await _localGameSave.Save(data);
        _saveCounter++;

        // 5번 이후 그 뒤에 Firebase에 한 번 저장한다.
        if (_saveCounter >= 5)
        {
            await _firebaseGameSave.Save(data);
            _saveCounter = 0;
        }
#else
        await _localGameSave.Save(data);
#endif
    }

    public async UniTask<GameSaveData> Load()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        var (localData, firebaseData) = await UniTask.WhenAll(_localGameSave.Load(), _firebaseGameSave.Load());

        // Timestamp 비교해서 최신 것을 반환한다.
        return GetLatestData(localData, firebaseData);
#else
        var localData = await _localGameSave.Load();
        return localData;
#endif
    }

    private GameSaveData GetLatestData(GameSaveData local, GameSaveData firebase)
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
