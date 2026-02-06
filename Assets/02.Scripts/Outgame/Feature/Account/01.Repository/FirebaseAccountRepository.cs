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
        if (PlayerPrefs.HasKey(email))
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

    public async UniTask Save(AccountSaveData data)
    {
        // Timestamp(Ticks)을 추가한다.
        data.Timestamp = DateTime.UtcNow.Ticks;

        string email = _auth.CurrentUser.Email;
        await _db.Collection("Account").Document(email).SetAsync(data);
    }

    public async UniTask<AccountSaveData> Load()
    {
        try
        {
            string email = _auth.CurrentUser.Email;
            DocumentSnapshot snapshot = await _db.Collection("Account").Document(email).GetSnapshotAsync();
            AccountSaveData data = snapshot.ConvertTo<AccountSaveData>();
            if (data != null)
            {
                return data;
            }
            return AccountSaveData.Default;
        }
        catch (Exception e)
        {
            Debug.LogError($"Account 불러오기에 실패했습니다" + e.Message);
            return AccountSaveData.Default;
        }
    }
}
