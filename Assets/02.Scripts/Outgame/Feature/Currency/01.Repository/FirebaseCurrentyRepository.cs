using UnityEngine;

public class FirebaseCurrentyRepository : ICurrencyRepository
{
    public void Save(CurrencySaveData saveData)
    {
        // 파이어베이스: 데이터를 서버에 저장하는 것이다.
        // 다음 주에 파이어베이스를 배우면 채운다.
    }

    public CurrencySaveData Load()
    {
        return CurrencySaveData.Default;
    }    
}
