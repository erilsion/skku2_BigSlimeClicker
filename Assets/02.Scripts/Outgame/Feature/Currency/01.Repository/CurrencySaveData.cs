using UnityEngine;

public class CurrencySaveData
{
    // 재화 관련 배열
    public double[] Currencies;
    public double[] EarnedTotals;

    // 재화 기본값
    public static CurrencySaveData Default => new CurrencySaveData()
    {
        Currencies = new double[(int)ECurrencyType.Count],
        EarnedTotals = new double[(int)ECurrencyType.Count]
    };
}
