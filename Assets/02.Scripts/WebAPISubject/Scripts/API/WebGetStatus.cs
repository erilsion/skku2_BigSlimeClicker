using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetStatus
{
    private const string API_KEY = "test_e3ff7acfde4849871d2d5c17a8b66c17028bbea2c0d0b370ab48a192b2fbf986efe8d04e6d233bd35cf2fabdeb93fb0d";

    // 캐릭터 정보를 웹에서 가져온다.
    public async UniTask<string> GetWebText(string url)
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

    // 캐릭터 이미지를 웹에서 가져온다.
    public async UniTask<Texture2D> GetWebTexture(string imageUrl)
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
}
