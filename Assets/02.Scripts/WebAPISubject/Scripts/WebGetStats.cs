using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class WebGetStats : MonoBehaviour
{
    private const string API_KEY = "test_e3ff7acfde4849871d2d5c17a8b66c17028bbea2c0d0b370ab48a192b2fbf986efe8d04e6d233bd35cf2fabdeb93fb0d";

    private async void Start()
    {
        string url = "https://open.api.nexon.com/maplestory/v1/character/basic?ocid=XXXX";
        string result = await GetWebText(url);
        Debug.Log(result);
    }

    private async UniTask<string> GetWebText(string url)
    {
        UnityWebRequest txt = UnityWebRequest.Get(url);
        txt.SetRequestHeader("x-nxopen-api-key", API_KEY);
        await txt.SendWebRequest();
        return txt.downloadHandler.text;
    }
}
