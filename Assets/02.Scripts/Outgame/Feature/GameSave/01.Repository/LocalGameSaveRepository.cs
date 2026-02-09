using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class LocalGameSaveRepository : IGameSaveRepository
{
    private const string KEY = "GameSave";

    public UniTask Save(GameSaveData data)
    {
        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
        return UniTask.CompletedTask;
    }

    public UniTask<GameSaveData> Load()
    {
        if (!PlayerPrefs.HasKey(KEY)) 
        {
            return UniTask.FromResult(GameSaveData.Default);
        }
        var json = PlayerPrefs.GetString(KEY);
        if (string.IsNullOrEmpty(json))
        {
            return UniTask.FromResult(GameSaveData.Default);
        }

        var data = JsonUtility.FromJson<GameSaveData>(json);
        return UniTask.FromResult(data ?? GameSaveData.Default);
    }
}
