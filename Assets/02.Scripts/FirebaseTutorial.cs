using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using NUnit.Framework;
using Firebase.Firestore;

public class FirebaseTutorial : MonoBehaviour
{
    private FirebaseApp _app = null;
    private FirebaseAuth _auth = null;
    private FirebaseFirestore _db = null;

    private void Start()
    {
        // 콜백 함수: 특정 이벤트가 발생하고 나면 자동으로 호출되는 함수이다.
        // 접속에 최소 1MS 이상의 시간이 걸릴 수 있다.
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if(task.Result == DependencyStatus.Available)
            {
                // 1. 파이어베이스 연결에 성공했다.
                _app = FirebaseApp.DefaultInstance;      // 파이어베이스 앱 모듈 가져오기
                _auth = FirebaseAuth.DefaultInstance;    // 파이어베이스 인증 모듈 가져오기
                _db = FirebaseFirestore.DefaultInstance; // 파이어베이스 데이터베이스 모듈 가져오기
                Debug.Log("Firebase 초기화 성공");
            }
            else 
            { 
                Debug.LogError($"Firebase 초기화 실패: {task.Result}");
            }
        });
    }

    public void Register(string email, string password)
    {
        _auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("회원 가입이 취소되었습니다.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("회원 가입에 실패했습니다: " + task.Exception);
                return;
            }

            // 파이어베이스 사용자 생성에 성공했다.
            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("회원 가입에 성공했습니다: {0} ({1})", result.User.DisplayName, result.User.UserId);
        });
    }

    private void Login(string email, string password)
    {
        _auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("로그인에 실패했습니다: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;

            // 로그인에 성공하면 반환되는 결과값의 유저와 auth 모듈의 CurrentUser가 둘 다 로그인한 유저로 같다.
            FirebaseUser resultuser = _auth.CurrentUser;
            FirebaseUser user = _auth.CurrentUser;

            Debug.LogFormat("로그인에 성공했습니다: {0} ({1})", result.User.Email, result.User.UserId);
        });
    }

    private void Logout()
    {
        _auth.SignOut();
        Debug.Log("로그아웃 되었습니다.");
    }

    private void CheckLoginStatus()
    {
        FirebaseUser user = _auth.CurrentUser;
        if(user == null)
        {
            Debug.Log("로그인 상태가 아닙니다.");
        }
        else
        {
            Debug.LogFormat("로그인 상태입니다: {0} ({1})", user.Email, user.UserId);
        }
    }

    private void SaveDog()
    {
        Dog Dog = new Dog("뽀삐", 4);

        _db.Collection("Dogs").AddAsync(Dog).ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully)
            {
                string documentId = task.Result.Id;
                Debug.Log("저장에 성공했습니다." + documentId);
            }
            else
            {
                Debug.LogError("저장에 실패했습니다: " + task.Exception);
            }
        });
    }

    private void Update()
    {
        if(_app == null)
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Register("erilsion@skku.re.kr", "12345678");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Login("erilsion@skku.re.kr", "12345678");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Logout();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CheckLoginStatus();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SaveDog();
        }
    }
}
