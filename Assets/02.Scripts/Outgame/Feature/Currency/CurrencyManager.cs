using System;
using UnityEngine;

// 오직 재화만 관리하는 클래스이다.
// 클린 아키텍처에서는 '서비스'라는 이름을 쓴다. (그러나 게임에서는 보통 '매니저'라고 표현한다.)
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    // CRUD
    // 재화 관리란 데이터에 대한 생성 / 조회 / 사용 / 소모 / 이벤트를 뜻한다.
    //             ㄴ 비즈니스 로직 (게임 로직): 데이터 사용에 대한 핵심 규칙이다.

    // 재화 데이터들(배열로 관리한다).
    private double[] _currencies = new double[(int)ECurrencyType.Count];

    public static event Action OnDataChanged;

    private void Awake()
    {
        Instance = this;
    }

    // 1. 재화를 조회한다.
    public double Get(ECurrencyType currencyType)
    {
        return _currencies[(int)currencyType];
    }

    // 어쩔 수 없는 재화 조회 편의 기능은 있어야 한다.
    public double Potion => Get(ECurrencyType.Potion);
    // public double Gem => Get(ECurrencyType.Gem); 등등 새로운 재화 생기면 enum에 추가하고 확장 가능하다.

    // 2. 재화를 추가한다.
    public void Add(ECurrencyType type, double amount)
    {
        _currencies[(int)type] += amount;

        OnDataChanged?.Invoke();
    }

    // 3. 재화를 소모한다.
    public bool TrySpend(ECurrencyType type, double amount)
    {
        if(_currencies[(int)type] >= amount)
        {
            _currencies[(int)type] -= amount;

            OnDataChanged?.Invoke();

            return true;
        }
        return false;
    }

    // 4. 돈이 있으세요?
    public bool CanAfford(ECurrencyType type, double amount)
    {
        return _currencies[(int)type] >= amount;
    }
}
