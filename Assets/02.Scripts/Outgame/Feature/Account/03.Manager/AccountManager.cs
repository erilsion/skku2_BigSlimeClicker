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

    // [SerializeField] private TextMeshProUGUI _messageText;

    private void Awake()
    {
        Instance = this;
    }

    public AuthResult TryLogin(string email, string password)
    {
        Account account = null;
        try
        {
            account = new Account(email, password);
        }
        catch (Exception ex)
        {
            // 유효성 검증 통과 못 하면 실패한다.
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "아이디 혹은 비밀번호를 확인해주세요!"
            };
        }
        // 가입한 적 없다면 실패한다.
        if (!PlayerPrefs.HasKey(email))
        {
            return new AuthResult
            {
                Success = false,
                ErrorMessage = "가입한 적이 없는 계정이에요!"
            };
        }
        return new AuthResult
        {
            Success = false,
            ErrorMessage = "로그인에 실패했어요!"
        };
    }

    public bool TryRegister(string email, string password) 
    {
        if (PlayerPrefs.HasKey(email))
        {
            return false;
        }

        try
        {
            Account account = new Account(email, password);
        }
        catch(Exception ex)
        {
            // 유효성 검증 통과 못 하면 실패한다.
            return false;
        }

        // 성공하면 저장한다.
        PlayerPrefs.SetString(email, password);
        return true; 
    }

    public void Logout()
    {

    }
}
