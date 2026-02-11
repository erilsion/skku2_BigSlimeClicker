using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetStatus
{
    private const string API_KEY = "test_e3ff7acfde4849871d2d5c17a8b66c17028bbea2c0d0b370ab48a192b2fbf986efe8d04e6d233bd35cf2fabdeb93fb0d";

    public async Task<string> GetOcid(string ocidUrl)
    {
        string json = await GetWebText(ocidUrl);
        MapleOcid data = JsonUtility.FromJson<MapleOcid>(json);
        return data.ocid;
    }

    // 캐릭터 기본 정보를 웹에서 가져온다.
    public async UniTask<MapleCharacter> GetCharacterInformation(string basicUrl)
    {
        string json = await GetWebText(basicUrl);
        return JsonUtility.FromJson<MapleCharacter>(json);
    }

    // 캐릭터 이미지를 웹에서 가져온다.
    public async UniTask<Texture> GetCharacterTexture(string imageUrl)
    {
        using var request = UnityWebRequestTexture.GetTexture(imageUrl);
        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            return null;
        }

        return DownloadHandlerTexture.GetContent(request);
    }

    // 캐릭터 세부 스탯 정보를 웹에서 가져온다.
    public async UniTask<MapleCharacterStat> GetStatInformation(string statUrl)
    {
        string json = await GetWebText(statUrl);
        return JsonUtility.FromJson<MapleCharacterStat>(json);
    }

    // URL으로 정보를 받아 string으로 전환해준다.
    private async UniTask<string> GetWebText(string url)
    {
        using var txt = UnityWebRequest.Get(url);
        txt.SetRequestHeader("x-nxopen-api-key", API_KEY);
        await txt.SendWebRequest();

        if (txt.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(txt.error);
            return null;
        }

        return txt.downloadHandler.text;
    }

    // 캐릭터 기본 정보와 관련된 클래스이다.
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

    // 캐릭터 세부 스탯과 관련된 클래스이다.
    [Serializable]
    public class MapleCharacterStat
    {
        public string date;
        public string character_class;
        public List<MapleFinalStat> final_stat;
        public int remain_ap;
    }

    // 최종 스탯은 배열 형태로 되어 있는 정보라 별도 클래스로 분리했다.
    [Serializable]
    public class MapleFinalStat
    {
        public string stat_name;
        public string stat_value;
    }

    [System.Serializable]
    public class MapleOcid
    {
        public string ocid;
    }
}
