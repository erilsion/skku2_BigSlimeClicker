using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static WebGetStatus;

public class MapleCharacterUI : MonoBehaviour
{
    private WebGetStatus _mapleAPI = new WebGetStatus();

    [SerializeField] private TextMeshProUGUI _basicText;
    [SerializeField] private TextMeshProUGUI _statText;
    [SerializeField] private RawImage _characterImage;

    private MapleCharacter _character;
    private MapleCharacterStat _characterStat;

    private const string BASIC_URL = "https://open.api.nexon.com/maplestory/v1/character/basic?ocid=16b1a1f2502eea2a9fcf32c8eaa6b2ea";
    private const string STAT_URL = "https://open.api.nexon.com/maplestory/v1/character/stat?ocid=16b1a1f2502eea2a9fcf32c8eaa6b2ea";

    private async void Start()
    {
        _character = await _mapleAPI.GetCharacterInformation(BASIC_URL);
        _characterStat = await _mapleAPI.GetStatInformation(STAT_URL);
        _characterImage.texture = await _mapleAPI.GetCharacterTexture(_character.character_image);

        BindBasicUI();
        BindStatUI();
    }

    // 생성 일자 정보에서 시간 정보를 제거하고 UI에 바인딩한다.
    private void BindBasicUI()
    {
        string dateOnly = _character.character_date_create.Split('T')[0];

        _basicText.text =
            $"캐릭터 이름: {_character.character_name}\n" +
            $"서버: {_character.world_name}\n" +
            $"성별: {_character.character_gender}\n" +
            $"직업: {_character.character_class}\n" +
            $"레벨: {_character.character_level}\n" +
            $"길드: {_character.character_guild_name}\n" +
            $"생성일자: {dateOnly}";
    }

    // 세부 스탯 관련 UI에 바인딩한다.
    private void BindStatUI()
    {
        Dictionary<string, string> statDictionary = new Dictionary<string, string>();

        foreach (var stat in _characterStat.final_stat)
        {
            statDictionary[stat.stat_name] = stat.stat_value;
        }

        _statText.text =
            $"전투력 : {GetStatFormatted(statDictionary, "전투력")}\n" +
            $"최소 공격력 : {GetStatFormatted(statDictionary, "최소 스탯공격력")}\n" +
            $"최대 공격력 : {GetStatFormatted(statDictionary, "최대 스탯공격력")}\n\n" +

            $"HP : {GetStatFormatted(statDictionary, "HP")}\n" +
            $"MP : {GetStatFormatted(statDictionary, "MP")}\n\n" +

            $"STR : {GetStatFormatted(statDictionary, "STR")}\n" +
            $"DEX : {GetStatFormatted(statDictionary, "DEX")}\n" +
            $"INT : {GetStatFormatted(statDictionary, "INT")}\n" +
            $"LUK : {GetStatFormatted(statDictionary, "LUK")}\n\n" +

            $"보스 데미지 : {GetStat(statDictionary, "보스 몬스터 데미지")}%\n" +
            $"방무 : {GetStat(statDictionary, "방어율 무시")}%\n" +
            $"크확 : {GetStat(statDictionary, "크리티컬 확률")}%\n" +
            $"크뎀 : {GetStat(statDictionary, "크리티컬 데미지")}%";
    }

    // 천 단위 구분기호를 추가하여 포맷팅된 문자열을 반환한다.
    private string GetStatFormatted(Dictionary<string, string> dictionary, string key)
    {
        if (!dictionary.TryGetValue(key, out string value))
        {
            return "0";
        }

        if (long.TryParse(value, out long number))
        {
            if (number < 1000)
            {
                return number.ToString();
            }
            else
            {
                return number.ToString("N0");
            }
        }
        return value;
    }

    // 키에 해당하는 스탯 값을 반환한다. 없으면 "0"을 반환한다.
    private string GetStat(Dictionary<string, string> dictionary, string key)
    {
        return dictionary.TryGetValue(key, out var value) ? value : "0";
    }
}
