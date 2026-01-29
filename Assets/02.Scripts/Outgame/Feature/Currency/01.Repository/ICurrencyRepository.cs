using UnityEditor.Overlays;
using UnityEngine;

// 저장소가 가져야 할 인터페이스이다.
public interface ICurrencyRepository
{
    public void Save(CurrencySaveData saveData);
    public CurrencySaveData Load();
}

