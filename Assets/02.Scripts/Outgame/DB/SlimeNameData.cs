using UnityEngine;
using System;

public class SlimeNameData : MonoBehaviour
{
    public static SlimeNameData Instance { get; private set; }

    public string Name { get; private set; } = "";
    public bool HasName => !string.IsNullOrWhiteSpace(Name);

    public event Action<string> OnNameChanged;

    private const string KEY = "SLIME_NAME";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void SetName(string name)
    {
        name = (name ?? "").Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        // 동일 이름이면 이벤트/저장을 스킵한다.
        if (Name == name)
        {
            return;
        }

        Name = name;
        Save();

        OnNameChanged?.Invoke(Name);
    }

    public void Load()
    {
        Name = PlayerPrefs.GetString(KEY, "");
        if (!string.IsNullOrWhiteSpace(Name))
        {
            OnNameChanged?.Invoke(Name);
        }
    }

    private void Save()
    {
        PlayerPrefs.SetString(KEY, Name);
        PlayerPrefs.Save();
    }

#if UNITY_EDITOR
    [ContextMenu("DEBUG/Clear Saved Name")]
    private void DebugClear()
    {
        PlayerPrefs.DeleteKey(KEY);
        Name = "";
        OnNameChanged?.Invoke(Name);
    }
#endif
}
