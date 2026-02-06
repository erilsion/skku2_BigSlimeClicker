using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class LocalAccountRepository : IAccountRepository
{
    public bool IsEmailAvailable(string email)
    {
        if (PlayerPrefs.HasKey(email))
        {
            return false;
        }
        return true;
    }
    public UniTask<AccountResult> Register(string email, string password)
    {
        if (!IsEmailAvailable(email))
        {
            // = return new UniTask.FromResult(new AccountResult { ... });
            return new UniTask<AccountResult>(new AccountResult
            {
                Success = false,
                ErrorMessage = "중복된 계정이에요!"
            });
        }

        // 암호화를 시킨다.
        string hashedPassword = AESCrypto.Encrypt(password);

        // 성공하면 저장한다.
        PlayerPrefs.SetString(email, hashedPassword);
        return new UniTask<AccountResult>(new AccountResult
        {
            Success = true,
            Account = new Account(email, password)
        });
    }

    public UniTask<AccountResult> Login(string email, string password)
    {
        // 가입한 적 없다면 실패한다.
        if (!PlayerPrefs.HasKey(email))
        {
            return new UniTask<AccountResult>(new AccountResult
            {
                Success = false,
                ErrorMessage = "가입한 적이 없는 계정이에요!"
            });
        }

        // 비밀번호가 틀렸다면 실패한다.
        string myPassword = PlayerPrefs.GetString(email);
        string decryptedPassword = AESCrypto.Decrypt(myPassword);

        if (decryptedPassword != password)
        {
            return new UniTask<AccountResult>(new AccountResult
            {
                Success = false,
                ErrorMessage = "아이디와 비밀번호를 확인해주세요!"
            });
        }
        return new UniTask<AccountResult>(new AccountResult
        {
            Success = true,
            Account = new Account(email, password)
        });
    }

    public void Logout()
    {
        Debug.Log("로그아웃 됐습니다.");
    }
}
