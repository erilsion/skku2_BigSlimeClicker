using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WebGetStatus : MonoBehaviour
{
    private const string API_KEY = "test_e3ff7acfde4849871d2d5c17a8b66c17028bbea2c0d0b370ab48a192b2fbf986efe8d04e6d233bd35cf2fabdeb93fb0d";
    private MapleCharacter _character;
    private MapleCharacterStat _characterStat;

    [SerializeField] private TextMeshProUGUI _basicText;
    [SerializeField] private TextMeshProUGUI _statText;
    [SerializeField] private RawImage _characterImage;

    private async void Start()
    {
        string basic = "https://open.api.nexon.com/maplestory/v1/character/basic?ocid=16b1a1f2502eea2a9fcf32c8eaa6b2ea";
        string stat = "https://open.api.nexon.com/maplestory/v1/character/stat?ocid=16b1a1f2502eea2a9fcf32c8eaa6b2ea";
        string basicResult = await GetWebText(basic);
        string statResult = await GetWebText(stat);
        _character = JsonUtility.FromJson<MapleCharacter>(basicResult);
        string dateCreate = await PolishDataCreate(_character.character_date_create);
        _characterStat = JsonUtility.FromJson<MapleCharacterStat>(statResult);
        BindStatUI();
        _characterImage.texture = await GetWebTexture(_character.character_image);
        Debug.Log(basicResult);
        Debug.Log(statResult);
        _basicText.text =
        $"캐릭터 이름: {_character.character_name}\n" +
        $"서버: {_character.world_name}\n" +
        $"성별: {_character.character_gender}\n" +
        $"직업: {_character.character_class}\n" +
        $"레벨: {_character.character_level}\n" +
        $"길드: {_character.character_guild_name}\n" +
        $"생성일자: {dateCreate}";
        // _statText.text = ui 바인딩;
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

    private void BindStatUI()
    {
        Dictionary<string, string> statDictionary = new Dictionary<string, string>();

        foreach (var stat in _characterStat.final_stat)
            statDictionary[stat.stat_name] = stat.stat_value;

        _statText.text =
            $"전투력 : {GetStat(statDictionary, "전투력")}\n" +
            $"최소 공격력 : {GetStat(statDictionary, "최소 스탯공격력")}\n" +
            $"최대 공격력 : {GetStat(statDictionary, "최대 스탯공격력")}\n\n" +

            $"INT : {GetStat(statDictionary, "INT")}\n" +
            $"STR : {GetStat(statDictionary, "STR")}\n" +
            $"DEX : {GetStat(statDictionary, "DEX")}\n" +
            $"LUK : {GetStat(statDictionary, "LUK")}\n\n" +

            $"보스 데미지 : {GetStat(statDictionary, "보스 몬스터 데미지")}%\n" +
            $"방무 : {GetStat(statDictionary, "방어율 무시")}%\n" +
            $"크확 : {GetStat(statDictionary, "크리티컬 확률")}%\n" +
            $"크뎀 : {GetStat(statDictionary, "크리티컬 데미지")}%";
    }

    private string GetStat(Dictionary<string, string> dict, string key)
    {
        return dict.TryGetValue(key, out var value) ? value : "0";
    }

    [Serializable]
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

    [Serializable]
    public class MapleCharacterStat
    {
        public string date;
        public string character_class;
        public List<MapleFinalStat> final_stat;
        public int remain_ap;
    }

    [Serializable]
    public class MapleFinalStat
    {
        public string stat_name;
        public string stat_value;
    }
}
