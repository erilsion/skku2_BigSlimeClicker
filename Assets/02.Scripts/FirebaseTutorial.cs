using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Threading;
using UnityEngine;

public class FirebaseTutorial : MonoBehaviour
{
    private FirebaseApp _app = null;
    private FirebaseAuth _auth = null;
    private FirebaseFirestore _db = null;

    [SerializeField] private TMPro.TMP_Text _progressText;

    private async void Start()
    {
        // 이 씬이 시작되면 아래 내용을 단계적으로 수행한다.
        // 각 단계마다 ProgressText의 내용이 바뀐다. (ex. 로그아웃 완료)
        // 각 단계마다 Debug.Log로 완료를 알린다.

        Debug.Log("현재 CPU 번호" + Thread.CurrentThread.ManagedThreadId);

        // 1. 파이어베이스 초기화
        await InitFirebase();
        _progressText.text = "파이어베이스 초기화 완료";
        Debug.Log("파이어베이스 초기화 완료");

        Debug.Log("현재 CPU 번호" + Thread.CurrentThread.ManagedThreadId);

        // 2. 로그아웃
        Logout();
        _progressText.text = "로그아웃 완료";
        Debug.Log("로그아웃 완료");

        Debug.Log("현재 CPU 번호" + Thread.CurrentThread.ManagedThreadId);

        // 3. 재로그인
        await Login("erilsion@skku.re.kr", "12345678");
        _progressText.text = "로그인 완료";
        Debug.Log("로그인 완료");

        Debug.Log("현재 CPU 번호" + Thread.CurrentThread.ManagedThreadId);

        // 4. 강아지 추가 (전제조건: 파이어스토어 규칙에 로그인한 사람만 글 쓰기 가능)
        await SaveDog("해피", 10);
        _progressText.text = "강아지 추가 완료";
        Debug.Log("강아지 추가 완료");

        Debug.Log("현재 CPU 번호" + Thread.CurrentThread.ManagedThreadId);
    }

    private async UniTask InitFirebase()
    {
        // 콜백 함수: 특정 이벤트가 발생하고 나면 자동으로 호출되는 함수이다.
        // 접속에 최소 1MS 이상의 시간이 걸릴 수 있다.

        // 유니티는 MonoBehaviour 실행에 있어서 싱글 쓰레드를 사용한다.
        // Task 타입: 비동기에 대한 진행사항과 완료되었을 때의 결과값을 가지고 있는 객체이다.
        // - 결과값을 Result로 가지고 있고, 진행사항 및 에러값을 또 가지고 있다.

        // 이 작업은 유니티가 실행 중 CPU1에게 작업시킬 수도 있고, CPU2에게 작업시킬 수도 있다.
        // 작업이 완료되고 나서 유니티가 실행 중인 CPU1에서 작업을 이어나가는 게 아니라 CPU2에서
        // MonoBehaviour 작업을 이어나가려고 하면 CPU2는 유니티를 모르기 때문에 뻗어버린다.
        // 이것을 유니티는 쓰레드 세이프하지 않다고 표현한다. 그래서 Task 사용을 지양한다.
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

        try
        {
            if (status == DependencyStatus.Available)
            {
                // 1. 파이어베이스 연결에 성공했다.
                _app = FirebaseApp.DefaultInstance;      // 파이어베이스 앱 모듈 가져오기
                _auth = FirebaseAuth.DefaultInstance;    // 파이어베이스 인증 모듈 가져오기
                _db = FirebaseFirestore.DefaultInstance; // 파이어베이스 데이터베이스 모듈 가져오기
                Debug.Log("Firebase 초기화에 성공했습니다.");
            }
        }
        catch(FirebaseException e)
        {
            Debug.LogError($"Firebase 초기화 실패: {e.Message}");
        }
        catch(Exception e)
        {
            Debug.LogError($"실패: {e.Message}");
        }
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

    private async UniTask Login(string email, string password)
    {
        try
        {
            Firebase.Auth.AuthResult authResult = await _auth.SignInWithEmailAndPasswordAsync(email, password).AsUniTask();
            Debug.LogFormat("로그인에 성공했습니다: {0} ({1})", authResult.User.Email, authResult.User.UserId);
        }
        catch (FirebaseException e)
        {
            Debug.LogError("파이어베이스 로그인에 실패했습니다: " + e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("로그인에 실패했습니다: " + e.Message);
        }
        // 로그인에 성공하면 반환되는 결과값의 유저와 auth 모듈의 CurrentUser가 둘 다 로그인한 유저로 같다.
        FirebaseUser resultuser = _auth.CurrentUser;
        FirebaseUser user = _auth.CurrentUser;
    }

    private void Logout()
    {
        _auth.SignOut();
        Debug.Log("로그아웃을 완료했습니다.");
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

    private async UniTask SaveDog(string name, int age)
    {
        Dog Dog = new Dog(name, age);

        // Add vs Set
        // Add: 추가한다.
        // Set: 기존에 있으면 수정하고, 없으면 추가한다.

        try
        {
            DocumentReference reference = await _db.Collection("Dogs").AddAsync(Dog).AsUniTask();
            Debug.Log("저장에 성공했습니다." + reference.Id);
        }
        catch (FirebaseException e)
        {
            Debug.LogError("파이어베이스 저장에 실패했습니다: " + e.Message);
        }
        catch (Exception e)
        {
            Debug.LogError("저장에 실패했습니다: " + e.Message);
        }
    }

    private void LoadMyDog()
    {
        _db.Collection("Dogs").Document("강아지들").GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully)
            {
                var snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dog myDog = snapshot.ConvertTo<Dog>();
                    Debug.Log($"{myDog.Name}{myDog.Age}");
                }
                else
                {
                    Debug.LogError("데이터가 없습니다.");
                }
            }
            else
            {
                Debug.LogError("불러오기에 실패했습니다: " + task.Exception);
            }
        });
    }

    private void LoadDogs()
    {
        _db.Collection("Dogs").GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully)
            {
                var snapshots = task.Result;
                foreach (DocumentSnapshot snapshot in snapshots.Documents)
                {
                    Dog dog = snapshot.ConvertTo<Dog>();
                    Debug.Log($"{dog.Name}{dog.Age}");
                }
                Debug.Log("불러오기를 성공했습니다.");
            }
            else
            {
                Debug.LogError("불러오기에 실패했습니다: " + task.Exception);
            }
        });
    }

    private void DeleteDogs()
    {
        // 목표: 특정 강아지를 삭제한다.
        _db.Collection("Dogs").WhereEqualTo("Name", "뽀삐").GetSnapshotAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully)
            {
                var snapshots = task.Result;
                foreach (DocumentSnapshot snapshot in snapshots.Documents)
                {
                    Dog myDog = snapshot.ConvertTo<Dog>();
                    if(myDog.Name == "뽀삐")
                    {
                        _db.Collection("Dogs").Document(myDog.Id).DeleteAsync().ContinueWithOnMainThread(deleteTask =>
                        {
                            if(deleteTask.IsCompletedSuccessfully)
                            {
                                Debug.Log("삭제에 성공했습니다.");
                            }
                            else
                            {
                                Debug.LogError("삭제에 실패했습니다: " + deleteTask.Exception);
                            }
                        });
                    }
                }
                Debug.Log("불러오기를 성공했습니다.");
            }
            else
            {
                Debug.LogError("불러오기에 실패했습니다: " + task.Exception);
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
            SaveDog("뽀삐", 4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            LoadMyDog();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LoadDogs();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            DeleteDogs();
        }
    }
}
