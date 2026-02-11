using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class MapleCharacterInformation
{
    private WebGetStatus _web = new WebGetStatus();

    private const string OCID_URL =
    "https://open.api.nexon.com/maplestory/v1/id?character_name={0}";

    private const string BASIC_URL =
    "https://open.api.nexon.com/maplestory/v1/character/basic?ocid={0}";

    private const string STAT_URL =
    "https://open.api.nexon.com/maplestory/v1/character/stat?ocid={0}";

    public async UniTask<MapleCharacterBundle> GetCharacterInformation(string characterName)
    {
        // 한글 인식 처리를 위해 URL 인코딩을 수행한다.
        string searchName = UnityWebRequest.EscapeURL(characterName);

        // OCID 정보를 가져온다.
        string ocidJson = await _web.GetWebText(string.Format(OCID_URL, searchName));
        var ocidData = JsonUtility.FromJson<MapleCharacterOcid>(ocidJson);

        if (ocidData == null || string.IsNullOrEmpty(ocidData.ocid))
        {
            Debug.LogError("캐릭터 정보가 없습니다.");
            return null;
        }
        string ocid = ocidData.ocid;

        // 캐릭터의 기본 정보를 가져온다.
        string basicJson = await _web.GetWebText(string.Format(BASIC_URL, ocid));
        var basic = JsonUtility.FromJson<MapleCharacter>(basicJson);

        // 캐릭터의 세부 스탯 정보를 가져온다.
        string statJson = await _web.GetWebText(string.Format(STAT_URL, ocid));
        var stat = JsonUtility.FromJson<MapleCharacterStat>(statJson);

        // 캐릭터의 이미지를 가져온다.
        Texture2D texture = await _web.GetWebTexture(basic.character_image);

        // 모든 정보를 묶어서 반환한다.
        return new MapleCharacterBundle
        {
            Basic = basic,
            Stat = stat,
            Texture = texture
        };
    }
}
