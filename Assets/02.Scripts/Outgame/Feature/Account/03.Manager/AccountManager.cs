using System;
using TMPro;
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

    [Header("레포지토리 제공자")]
    [SerializeField] private GameObject _repositoryProvider;
    private IAccountRepository _repository;

    private void Awake()
    {
        Instance = this;
        _repository = _repositoryProvider.GetComponent<IAccountRepository>();
    }

    public AuthResult TryLogin(string email, string password)
    {
        try
        {
            Account account = new Account(email, password);
        }
        catch (Exception ex)
        {
            // 유효성 검증 통과 못 하면 실패한다.
            return new AuthResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }

        AuthResult result = _repository.Login(email, password);
        if (result.Success)
        {
            _currentAccount = result.Account;
            return new AuthResult
            {
                Success = true,
                Account = _currentAccount
            };
        }
        else
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = result.ErrorMessage
            };
        }
    }

    public AuthResult TryRegister(string email, string password) 
    {
        // 유효성 검사를 한다.
        try
        {
            Account account = new Account(email, password);
        }
        catch(Exception ex)
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }

        // 성공하면 저장한다.
        AuthResult result = _repository.Register(email, password);
        if (result.Success)
        {
            return new AuthResult
            {
                Success = true,
            };
        }
        else
        {
            return new AuthResult
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
