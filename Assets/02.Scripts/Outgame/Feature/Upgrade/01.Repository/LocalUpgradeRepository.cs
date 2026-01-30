using UnityEngine;

public class LocalUpgradeRepository : IUpgradeRepository
{
    public void Save(UpgradeSaveData saveData)
    {
        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            var type = (EUpgradeType)i;
            PlayerPrefs.SetString(type.ToString(), saveData.Upgrades[i].ToString("G17"));
        }
    }

    public UpgradeSaveData Load()
    {
        UpgradeSaveData data = UpgradeSaveData.Default;

        for (int i = 0; i < (int)EUpgradeType.Count; i++)
        {
            if (PlayerPrefs.HasKey(i.ToString()))
            {
                data.Upgrades[i] = double.Parse(PlayerPrefs.GetString(i.ToString(), "0"));
            }
        }

        return data;
    }
}
