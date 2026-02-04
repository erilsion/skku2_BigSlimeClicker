using UnityEngine;
using Cysharp.Threading.Tasks;

public class LocalUpgradeRepository : IUpgradeRepository
{
    private readonly string _userId;

    public LocalUpgradeRepository(string userId)
    {
        _userId = userId;
    }

    public async UniTaskVoid Save(UpgradeSaveData saveData)
    {
        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;
            PlayerPrefs.SetString(
                type.ToString(),
                saveData.Upgrades[i].ToString("G17",
                System.Globalization.CultureInfo.InvariantCulture)
            );
        }

        PlayerPrefs.Save();
    }

    public async UniTask<UpgradeSaveData> Load()
    {
        UpgradeSaveData data = UpgradeSaveData.Default;

        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;
            string key = type.ToString();

            if (PlayerPrefs.HasKey(key))
            {
                data.Upgrades[i] =
                    double.Parse(PlayerPrefs.GetString(key),
                    System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        return data;
    }
}
