using Cysharp.Threading.Tasks;
using UnityEngine;

public class SaveLoadUI : MonoBehaviour
{
    private HybridRepository _repository;

    private void Awake()
    {
        _repository = new HybridRepository();
    }
    public void OnClickSave()
    {
        var data = new GameSaveData
        {
            Timestamp = System.DateTime.UtcNow.Ticks,
            // Currency = CurrencyManager.Instance.ToSaveData(),
            // Upgrade = UpgradeManager.Instance.ToSaveData()
        };

        _repository.Save(data).Forget();
    }

    public void OnClickLoad()
    {
        LoadAsync().Forget();
    }

    private async UniTaskVoid LoadAsync()
    {
        var data = await _repository.Load();
        // CurrencyManager.Instance.Apply(data.Currency);
        // UpgradeManager.Instance.Apply(data.Upgrade);

        Debug.Log($"현재 틱: {data.Timestamp}");
    }
}
