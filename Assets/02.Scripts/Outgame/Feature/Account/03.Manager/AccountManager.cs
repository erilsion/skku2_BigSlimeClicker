using UnityEngine;

// 매니저의 역할
// 1. 도메인 관리: 생성 / 조회 / 수정 / 삭제와 같은 비즈니스 로직을 관리한다.
// 2. 외부와의 소통 창구 역할을 한다.
public class AccountManager : MonoBehaviour
{
    public AccountManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
