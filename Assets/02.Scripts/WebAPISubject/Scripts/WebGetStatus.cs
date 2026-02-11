using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebGetStatus : MonoBehaviour
{
    private const string API_KEY = "test_e3ff7acfde4849871d2d5c17a8b66c17028bbea2c0d0b370ab48a192b2fbf986efe8d04e6d233bd35cf2fabdeb93fb0d";
    private MapleCharacter _character;

    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private RawImage _characterImage;

    private async void Start()
    {
        string url = "https://open.api.nexon.com/maplestory/v1/character/basic?ocid=16b1a1f2502eea2a9fcf32c8eaa6b2ea";
        string result = await GetWebText(url);
        _character = JsonUtility.FromJson<MapleCharacter>(result);
        _characterImage.texture = await GetWebTexture(_character.character_image);
        string dateCreate = await PolishDataCreate(_character.character_date_create);
        Debug.Log(result);
        _statusText.text =
        $"캐릭터 이름: {_character.character_name}\n" +
        $"서버: {_character.world_name}\n" +
        $"성별: {_character.character_gender}\n" +
        $"직업: {_character.character_class}\n" +
        $"레벨: {_character.character_level}\n" +
        $"길드: {_character.character_guild_name}\n" +
        $"생성일자: {dateCreate}";
    }

    private async UniTask<string> GetWebText(string url)
    {
        UnityWebRequest txt = UnityWebRequest.Get(url);
        txt.SetRequestHeader("x-nxopen-api-key", API_KEY);
        await txt.SendWebRequest();
        return txt.downloadHandler.text;
    }

    private async UniTask<Texture> GetWebTexture(string json)
    {
        using var request = UnityWebRequestTexture.GetTexture(json);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        return DownloadHandlerTexture.GetContent(request);
    }

    private async UniTask<string> PolishDataCreate(string dateString)
    {
        string rawDate = dateString;
        string dateOnly = rawDate.Split('T')[0];
        await UniTask.Yield();
        return dateOnly;
    }

    [System.Serializable]
    public class MapleCharacter
    {
        public string date;
        public string character_name;
        public string world_name;
        public string character_gender;
        public string character_class;
        public string character_class_level;
        public int character_level;
        public long character_exp;
        public string character_exp_rate;
        public string character_guild_name;
        public string character_image;
        public string character_date_create;
        public string access_flag;
        public string liberation_quest_clear;
    }
}
