#if UNITY_WEBGL && !UNITY_EDITOR
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using Firebase.Firestore;
using System;
using UnityEngine;

public class FirebaseAccountRepository : IAccountRepository
{
    private FirebaseFirestore _db = FirebaseFirestore.DefaultInstance;
    private FirebaseAuth _auth;

    public FirebaseAccountRepository()
    {
        _auth = FirebaseAuth.DefaultInstance;
    }

    public bool IsEmailAvailable(string email)
    {
        if (_auth.CurrentUser.Email == email)
        {
            return false;
        }
        return true;
    }

    public async UniTask<AccountResult> Register(string email, string password)
    {
        if (!IsEmailAvailable(email))
        {
            return new AccountResult
            {
                Success = false,
                ErrorMessage = "중복된 계정이에요!"
            };
        }

        try
        {
            AuthResult result = await _auth.CreateUserWithEmailAndPasswordAsync(email, password).AsUniTask();
            return new AccountResult()
            {
                Success = true,
            };
        }
        catch (Exception e)
        {
            return new AccountResult()
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }

    }

    public async UniTask<AccountResult> Login(string email, string password)
    {
        try
        {
            Firebase.Auth.AuthResult result = await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            return new AccountResult()
            {
                Success = true,
            };
        }
        catch (Exception e)
        {
            return new AccountResult()
            {
                Success = false,
                ErrorMessage = e.Message
            };
        }
    }

    public void Logout()
    {
        _auth.SignOut();
    }
}
#endif
