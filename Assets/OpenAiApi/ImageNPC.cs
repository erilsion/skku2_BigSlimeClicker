using OpenAI;
using OpenAI.Images;
using OpenAI.Models;
using UnityEngine;
using UnityEngine.UI;

public class ImageNPC : MonoBehaviour
{
    [SerializeField] private ApiKeyConfig _config;
    [SerializeField] private RawImage _displayImage;

    private async void Start()
    {
        if (_config == null)
        {
            return;
        }
        if (_displayImage == null)
        {
            return;
        }
        string prompt = "맑은 하늘 아래 넓은 들판이 있고, 거대하고 푸르른 나무 밑에 있는 집 같은 아지트 건물을 그려줘.";

        // 1. ChatGPT 사이트에 API_KEY로 로그인한다.
        var api = new OpenAIClient(_config.OpenAIKey);

        // 2. 이미지 생성 요청 내용을 작성한다.
        var request = new ImageGenerationRequest(
            prompt: prompt,
            model: Model.GPT_Image_1
            );

        // 3. 요청을 보내고 응답을 받는다.
        var results = await api.ImagesEndPoint.GenerateImageAsync(request);
        _displayImage.texture = results[0].Texture;
    }
}
