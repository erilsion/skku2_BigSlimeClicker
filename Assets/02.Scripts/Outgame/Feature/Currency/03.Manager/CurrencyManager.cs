using System;
using System.Linq;
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
    private Currency[] _currencies = new Currency[(int)ECurrencyType.Count];
    private Currency[] _earnedTotals = new Currency[(int)ECurrencyType.Count];

    public static event Action OnDataChanged;

    // 저장소
    // 의존: 한 객체가 동작하기 위해서 다른 객체를 참조하는 것을 뜻한다.
    // DIP: 구현체에 의존하지 말고 약속에 의존하라는 뜻이다.
    private ICurrencyRepository _repository;

    private void Awake()
    {
        Instance = this;

        _repository = new LocalCurrencyRepository(AccountManager.Instance.Email);
    }

    private void Start()
    {
        var loaded = _repository.Load();
        for (int i = 0; i < _currencies.Length; i++)
        {
            _currencies[i] = loaded.Currencies[i];
        }
        for (int i = 0; i < _earnedTotals.Length; i++)
        {
            _earnedTotals[i] = loaded.EarnedTotals[i];
        }
        OnDataChanged?.Invoke();
    }

    // 1. 재화를 조회한다.
    public Currency Get(ECurrencyType currencyType)
    {
        return _currencies[(int)currencyType];
    }
    public Currency GetEarnedTotal(ECurrencyType type)
    {
        return _earnedTotals[(int)type];
    }

    // 어쩔 수 없이 재화 조회 편의 기능은 있어야 한다.
    public Currency Potion => Get(ECurrencyType.Potion);
    // public Currency Gem => Get(ECurrencyType.Gem); 등등 새로운 재화 생기면 enum에 추가하고 확장 가능하다.

    // 2. 재화를 추가한다.
    public void Add(ECurrencyType type, Currency amount)
    {
        if (amount <= 0)
        {
            return;
        }
        _currencies[(int)type] += amount;
        _earnedTotals[(int)type] += amount;

        Save();

        OnDataChanged?.Invoke();
    }

    // 3. 재화를 소모한다.
    public bool TrySpend(ECurrencyType type, Currency amount)
    {
        if(_currencies[(int)type] >= amount)
        {
            _currencies[(int)type] -= amount;

            Save();

            OnDataChanged?.Invoke();
            return true;
        }
        return false;
    }

    // 4. 돈이 있으세요?
    public bool CanAfford(ECurrencyType type, Currency amount)
    {
        return _currencies[(int)type] >= amount;
    }

    private void Save()
    {
        var saveData = new CurrencySaveData
        {
            Currencies = new double[_currencies.Length],
            EarnedTotals = new double[_earnedTotals.Length],
        };

        for (int i = 0; i < _currencies.Length; i++)
        {
            saveData.Currencies[i] = (double)_currencies[i];
            saveData.EarnedTotals[i] = (double)_earnedTotals[i];
        }
        _repository.Save(saveData);
    }

    // 도대체 관리라는 책임은 어디까지인가?

    // 저장하는 방식은 여러 가지 있다. -> 저장과 로드는 레포지토리에서 관리하게 한다.
    // 1. PlayerPrefs + double / string
    // 2. PlayerPrefs + double -> Json
    // 3. SCV / Json으로 저장한다.
    // 4. 서버 혹은 DB에 저장한다.
    // 5. 유니티에서는 3번, 빌드 이후에는 4번 방식으로 저장 되게 한다(예시).
    // 6. Save를 호출하면 Save가 더이상 호출되지 않은지 0.6초 지난 후 세이브를 한다(연타 방지).
}
