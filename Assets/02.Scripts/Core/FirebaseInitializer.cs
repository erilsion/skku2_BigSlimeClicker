#if !UNITY_WEBGL || UNITY_EDITOR
using Cysharp.Threading.Tasks;
using Firebase;
using System;
using UnityEngine;

public class FirebaseInitializer : MonoBehaviour
{
    public static FirebaseInitializer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 아랫쪽에서 실행할 코드가 없기 때문에 await를 붙이지 않아도 된다.
        // 그럴 때 Forget()를 붙여준다. (Fire-and-Forget 패턴: 비동기 작업을 시작만 하고, 결과는 기다리지 않겠다.)
        InitFirebase().Forget();
    }

    private async UniTask InitFirebase()
    {
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

        try
        {
            if (status == DependencyStatus.Available)
            {
                // 1. 파이어베이스 연결에 성공했다.
                Debug.Log("Firebase 초기화에 성공했습니다.");
            }
        }
        catch (FirebaseException e)
        {
            Debug.LogError($"Firebase 초기화 실패: {e.Message}");
        }
        catch (Exception e)
        {
            Debug.LogError($"실패: {e.Message}");
        }
    }
}
#endif
