using UnityEngine;
using Cysharp.Threading.Tasks;

// 데이터의 영속성(저장과 불러오기)에 대한 책임은 '레포지토리'가 가지고 있다.
// ㄴ 비즈니스 로직과 분리한다.

// 비즈니스 로직은 매니저에게, 저장 로직은 레포지토리에게 맡긴다.
// 코드가 깔끔해지고 유지 보수가 쉬워진다.
public class LocalCurrencyRepository: ICurrencyRepository
{
    private readonly string _userId;

    public LocalCurrencyRepository(string userId)
    {
        _userId = userId;
    }

    public async UniTaskVoid Save(CurrencySaveData saveData)
    {
        // 어떻게든 Save한다.
        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;

            // 잔액
            PlayerPrefs.SetString(MakeBalanceKey(type), saveData.Currencies[i].ToString("G17"));

            // 누적 획득량
            PlayerPrefs.SetString(MakeEarnedKey(type), saveData.EarnedTotals[i].ToString("G17"));
        }
        PlayerPrefs.Save();
    }

    public async UniTask<CurrencySaveData> Load()
    {
        // 어떻게든 Load한다.
        CurrencySaveData data = CurrencySaveData.Default;

        for (int i = 0; i < (int)ECurrencyType.Count; i++)
        {
            var type = (ECurrencyType)i;

            // 잔액
            string balanceKey = MakeBalanceKey(type);
            if (PlayerPrefs.HasKey(balanceKey))
            {
                data.Currencies[i] = double.Parse(PlayerPrefs.GetString(balanceKey, "0"));
            }

            // 누적 획득량
            string earnedKey = MakeEarnedKey(type);
            if (PlayerPrefs.HasKey(earnedKey))
            {
                data.EarnedTotals[i] = double.Parse(PlayerPrefs.GetString(earnedKey, "0"));
            }
        }

        return data;
    }

    private string MakeBalanceKey(ECurrencyType type)
    {
        return $"{_userId}_Currency_{type}_Balance";
    }

    private string MakeEarnedKey(ECurrencyType type)
    {
        return $"{_userId}_Currency_{type}_Earned";
    }
}
