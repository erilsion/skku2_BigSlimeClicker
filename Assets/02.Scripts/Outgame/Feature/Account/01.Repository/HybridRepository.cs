using Cysharp.Threading.Tasks;
using UnityEngine;

public class HybridRepository : IAccountRepository, IGameSaveRepository
{
    private readonly LocalAccountRepository _localAccount;
    private readonly FirebaseAccountRepository _firebaseAccount;
    private readonly LocalGameSaveRepository _localGameSave;
    private readonly FirebaseGameSaveRepository _firebaseGameSave;

    private int _saveCounter = 0;

    public HybridRepository()
    {
        _localAccount = new LocalAccountRepository();
        _firebaseAccount = new FirebaseAccountRepository();
        _localGameSave = new LocalGameSaveRepository();
        _firebaseGameSave = new FirebaseGameSaveRepository();
    }

    public bool IsEmailAvailable(string email)
    {
        return _localAccount.IsEmailAvailable(email);
    }

    public async UniTask<AccountResult> Register(string email, string password)
    {
        var result = await _firebaseAccount.Register(email, password);
        if (result.Success)
        {
            await _localAccount.Register(email, password);
        }
        return result;
    }

    public async UniTask<AccountResult> Login(string email, string password)
    {
        return await _firebaseAccount.Login(email, password);
    }

    public void Logout()
    {
        _firebaseAccount.Logout();
        _localAccount.Logout();
    }

    public async UniTask Save(GameSaveData data)
    {
        // 로컬에 저장한다.
        await _localGameSave.Save(data);
        _saveCounter++;

        // 5번 이후 그 뒤에 Firebase에 한 번 저장한다.
        if (_saveCounter >= 5)
        {
            await _firebaseGameSave.Save(data);
            _saveCounter = 0;
        }
    }

    public async UniTask<GameSaveData> Load()
    {
        var (localData, firebaseData) = await UniTask.WhenAll(_localGameSave.Load(), _firebaseGameSave.Load());

        // Timestamp 비교해서 최신 것을 반환한다.
        return GetLatestData(localData, firebaseData);
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
