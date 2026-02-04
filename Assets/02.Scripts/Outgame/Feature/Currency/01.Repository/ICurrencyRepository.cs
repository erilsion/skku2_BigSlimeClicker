using UnityEngine;
using Cysharp.Threading.Tasks;

// 저장소가 가져야 할 인터페이스이다.
public interface ICurrencyRepository
{
    public UniTaskVoid Save(CurrencySaveData saveData);
    public UniTask<CurrencySaveData> Load();
}

