using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Cysharp.Threading.Tasks;

public class WebGetTextTest : MonoBehaviour
{
    // HTTP 프로토콜을 이용해서 웹 서버에게 데이터 작업을 요청할 수 있다.
    // 작업 요청은 크게 4가지 약속이 있다.
    // 1. GET : 데이터 내놔
    // 2. POST : 데이터 줄게
    // 3. PUT : 데이터 수정해 줘
    // 4. DELETE : 데이터 삭제해 줘

    private async void Start()
    {
        // 서버에게 데이터 내놔 하는 작업은 비동기 작업이다.
        string result = await GetWebText("https://www.google.com/search?q=%EB%A7%88%EB%A6%AC%EC%98%A4&oq=%EB%A7%88%EB%A6%AC%EC%98%A4&gs_lcrp=EgZjaHJvbWUqEAgAEAAYgwEY4wIYsQMYgAQyEAgAEAAYgwEY4wIYsQMYgAQyDQgBEC4YgwEYsQMYgAQyBwgCEC4YgAQyBwgDEAAYgAQyBggEEAAYAzIHCAUQLhiABDIHCAYQLhiABDIKCAcQLhixAxiABDIHCAgQABiABDIHCAkQABiABNIBCDUyMDlqMGo3qAIAsAIA&sourceid=chrome&ie=UTF-8");
        Debug.Log(result);

        // 서버에게 데이터 내놔 하는 작업은 비동기이므로 코루틴을 이용했다. -> 현업에서는 흠?
        // StartCoroutine(GetText());
    }

    private async UniTask<string> GetWebText(string url)
    {
        var txt = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.text;
        return txt;
    }

    IEnumerator GetText()
    {
        // URL이란 웹서버 어떤 '자원(페이지/이미지/파일/데이터/API)'이 있는 위치를 가리키는 주소이다.
        UnityWebRequest www = UnityWebRequest.Get("https://www.google.com/search?q=%EB%A7%88%EB%A6%AC%EC%98%A4&oq=%EB%A7%88%EB%A6%AC%EC%98%A4&gs_lcrp=EgZjaHJvbWUqEAgAEAAYgwEY4wIYsQMYgAQyEAgAEAAYgwEY4wIYsQMYgAQyDQgBEC4YgwEYsQMYgAQyBwgCEC4YgAQyBwgDEAAYgAQyBggEEAAYAzIHCAUQLhiABDIHCAYQLhiABDIKCAcQLhixAxiABDIHCAgQABiABDIHCAkQABiABNIBCDUyMDlqMGo3qAIAsAIA&sourceid=chrome&ie=UTF-8");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
