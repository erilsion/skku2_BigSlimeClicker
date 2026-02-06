using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

// 매니저의 역할
// 1. 도메인 관리: 생성 / 조회 / 수정 / 삭제와 같은 비즈니스 로직을 관리한다.
// 2. 외부와의 소통 창구 역할을 한다.
public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }

    private Account _currentAccount = null;
    public bool IsLogin => _currentAccount != null;
    public string Email => _currentAccount?.Email ?? string.Empty;

    private IAccountRepository _repository;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        _repository = new HybridAccountRepository();
    }

    private void Start()
    {
        // UniTask와 관련된 초기화 패턴 관리 용도이다.
        StartAsync().Forget();
    }

    private async UniTask StartAsync()
    {
        await TryAutoLogin();
    }

    private async UniTask TryAutoLogin()
    {
        var savedData = await _repository.Load();

        if (savedData == null || string.IsNullOrEmpty(savedData.Email))
        {
            Debug.LogWarning("자동 로그인 데이터가 없어요.");
            return;
        }

        string plainPassword;
        try
        {
            plainPassword = AESCrypto.Decrypt(savedData.EncryptedPassword);
        }
        catch
        {
            Debug.LogWarning("저장된 비밀번호 복호화에 실패했어요. 자동로그인을 스킵할게요!");
            return;
        }

        var result = await TryLogin(savedData.Email, savedData.EncryptedPassword);
        if (result.Success)
        {
            Debug.Log("자동 로그인 성공!");
        }
        else
        {
            Debug.LogWarning("자동 로그인 실패: " + result.ErrorMessage);
        }
    }

    public async UniTask<AccountResult> TryLogin(string email, string password)
    {
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception ex)
        {
            // 유효성 검증 통과 못 하면 실패한다.
            return new AccountResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }

        AccountResult result = await _repository.Login(email, password);

        if (result.Success)
        {
            _currentAccount = new Account(email, password);

            // 자동로그인용 (AES) 데이터를 저장한다.
            var saveData = new AccountSaveData
            {
                Email = email,
                EncryptedPassword = AESCrypto.Encrypt(password),
            };
            await _repository.Save(saveData);
        }

        return result;
    }

    public async UniTask<AccountResult> TryRegister(string email, string password) 
    {
        // 유효성 검사를 한다.
        try
        {
            Account account = new Account(email, password);
        }
        catch(Exception ex)
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }

        // 성공하면 저장한다.
        AccountResult result = await _repository.Register(email, password);
        if (result.Success)
        {
            return new AccountResult
            {
                Success = true,
            };
        }
        else
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = result.ErrorMessage
            };
        }
    }

    public void Logout()
    {
        _repository.Logout();
    }
}
