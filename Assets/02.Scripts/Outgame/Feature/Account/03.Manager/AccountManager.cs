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

    private IAccountRepository _accountRepository;
    private IGameSaveRepository _gameSaveRepository;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        _accountRepository = new HybridRepository();
        _gameSaveRepository = new HybridRepository();
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

        AccountResult result = await _accountRepository.Login(email, password);

        if (result.Success)
        {
            _currentAccount = new Account(email, password);

            // AES 데이터를 저장한다.
            var saveData = new AccountSaveData
            {
                Email = email,
                EncryptedPassword = AESCrypto.Encrypt(password),
            };
            return result;
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
        AccountResult result = await _accountRepository.Register(email, password);
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
        _accountRepository.Logout();
    }
}
