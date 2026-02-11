using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapleCharacterUI : MonoBehaviour
{
    [Header("캐릭터 이름 입력창")]
    [SerializeField] private TMP_InputField _characterNameInput;
    [SerializeField] private Button _searchButton;

    [Header("캐릭터 정보 출력창")]
    [SerializeField] private TextMeshProUGUI _basicText;
    [SerializeField] private TextMeshProUGUI _statText1;
    [SerializeField] private TextMeshProUGUI _statText2;
    [SerializeField] private RawImage _characterImage;

    private MapleCharacterInformation _mapleAPI = new MapleCharacterInformation();

    private void Start()
    {
        // 입력창에서 Enter키를 누르면 검색 버튼이 클릭되도록 설정한다. (InputField의 onSubmit 이벤트 활용)
        _characterNameInput.onSubmit.AddListener(OnSubmit);
    }

    private void OnSubmit(string value)
    {
        _searchButton.onClick.Invoke();
    }

    public async void LoadCharacter()
    {
        var data = await _mapleAPI.GetCharacterInformation(_characterNameInput.text);

        if (data == null)
        {
            Debug.LogError("캐릭터 정보를 불러오지 못했습니다.");
            return;
        }

        BindImageUI(data.Texture);
        BindBasicUI(data.Basic);
        BindStatUI(data.Stat);
    }

    private void BindImageUI(Texture texture)
    {
        _characterImage.texture = texture;
        _characterImage.color = Color.white;
    }

    // 생성 일자 정보에서 시간 정보를 제거하고 UI에 바인딩한다.
    private void BindBasicUI(MapleCharacter data)
    {
        string dateOnly = data.character_date_create.Split('T')[0];

        _basicText.text =
            $"캐릭터 이름: {data.character_name}\n" +
            $"서버: {data.world_name}\n" +
            $"성별: {data.character_gender}\n" +
            $"직업: {data.character_class}\n" +
            $"전직: {data.character_class_level}차\n" +
            $"레벨: {data.character_level}\n" +
            $"길드: {data.character_guild_name}\n" +
            $"생성일자: {dateOnly}";
    }

    // 세부 스탯 관련 UI에 바인딩한다.
    private void BindStatUI(MapleCharacterStat data)
    {
        Dictionary<string, string> statDictionary = new Dictionary<string, string>();

        foreach (var stat in data.final_stat)
        {
            statDictionary[stat.stat_name] = stat.stat_value;
        }

        _statText1.text =
            $"전투력 : {GetStatFormatted(statDictionary, "전투력")}\n" +
            $"최소 공격력 : {GetStatFormatted(statDictionary, "최소 스탯공격력")}\n" +
            $"최대 공격력 : {GetStatFormatted(statDictionary, "최대 스탯공격력")}\n\n" +

            $"HP : {GetStatFormatted(statDictionary, "HP")}\n" +
            $"MP : {GetStatFormatted(statDictionary, "MP")}\n\n" +

            $"STR : {GetStatFormatted(statDictionary, "STR")}\n" +
            $"DEX : {GetStatFormatted(statDictionary, "DEX")}\n" +
            $"INT : {GetStatFormatted(statDictionary, "INT")}\n" +
            $"LUK : {GetStatFormatted(statDictionary, "LUK")}\n\n" +

            $"공격력: {GetStatFormatted(statDictionary, "공격력")}\n" +
            $"마력 : {GetStatFormatted(statDictionary, "마력")}\n" +
            $"방어력 : {GetStatFormatted(statDictionary, "방어력")}";

        _statText2.text =
            $"데미지 : {GetStat(statDictionary, "데미지")}%\n" +
            $"일반 데미지 : {GetStat(statDictionary, "일반 몬스터 데미지")}%\n" +
            $"보스 데미지 : {GetStat(statDictionary, "보스 몬스터 데미지")}%\n" +
            $"최종 데미지 : {GetStat(statDictionary, "최종 데미지")}%\n\n" +

            $"방어율 무시 : {GetStat(statDictionary, "방어율 무시")}%\n" +
            $"크리티컬 확률 : {GetStat(statDictionary, "크리티컬 확률")}%\n" +
            $"크리티컬 데미지 : {GetStat(statDictionary, "크리티컬 데미지")}%\n\n" +

            $"이동 속도 : {GetStatFormatted(statDictionary, "이동속도")}\n" +
            $"점프력 : {GetStatFormatted(statDictionary, "점프력")}\n\n" +

            $"스타포스 : {GetStatFormatted(statDictionary, "스타포스")}\n" +
            $"아케인포스 : {GetStatFormatted(statDictionary, "아케인포스")}\n" +
            $"어센틱포스 : {GetStatFormatted(statDictionary, "어센틱포스")}";
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
