using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using OpenAI.Audio;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatNPC : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultTextUI;
    [SerializeField] private TMP_InputField _promptTextField; // 프롬프트: AI에 우리 요청 사항을 담은 텍스트이다.
    [SerializeField] private Button _sendButton;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private ApiKeyConfig _config;

    // 대화 내용을 기억할 콘텍스트이다.
    // 메시지는 누적된다. (Request 보낼 때마다 토큰이 기하급수적으로 증가한다!)
    private List<Message> _message = new List<Message>();

    // 현업에서는 토큰량을 줄이기 위해서 여러 가지 기법을 쓴다. (AI 엔지니어의 주 역할이다.)
    // - 일정 개수가 넘으면 과거의 기억을 지워가는 방식 (최근 N개만 기억하는 방식)
    // - 일정 개수가 넘으면 압축하는 방식 (기존 내용을 요약하는 방식 등)
    // - '벡터 DB'(데이터를 벡터화해서) 저장해두고 검색하는 방식 (RAG)

    // API_KEY를 숨기는 방법
    // 1. 환경 변수를 이용한 방식
    // 2. gitignore를 이용한 방식
    // 3. 깃허브 시크릴 파일을 이용한 방식

    private void Start()
    {
        // NPC 모드 지침을 추가한다. (역할, 목적, 표현)
        string systemMessage = string.Empty;
        systemMessage += "역할: 너는 이제부터 게임 NPC다. 자신을 실제 게임 세상 속 고양이 인간이라고 생각한다.";
        systemMessage += "목적: 실제 사람처럼 대화하는 게임 NPC 모드";
        systemMessage += "표현: 말 끝마다 '냥' 또는 '냥냥'을 붙인다. 항상 200글자 이내로 답변한다.";

        _message.Add(new Message(Role.System, systemMessage));

        // 버튼 클릭 이벤트이다.
        _sendButton.onClick.AddListener(Send);
    }

    private async void Send()
    {
        // 프롬프트(AI에게 요청하는 내용을 담은 텍스트)를 읽어온다.
        string prompt = _promptTextField.text;
        if (string.IsNullOrEmpty(prompt))
        {
            return;
        }

        // 0. 버튼을 잠근다.
        _sendButton.interactable = false;

        // 1. ChatGPT 사이트에 API_KEY로 로그인한다.
        var api = new OpenAIClient(_config.OpenAIKey);

        // 2. 프롬프트를 작성한 후, 콘텍스트에 담는다.
        _message.Add(new Message(Role.User, prompt));

        // 3. 모델을 선택하고 요청을 보낸다.
        var chatRequest = new ChatRequest(_message, Model.GPT4oMini, temperature: 0);

        // 4. 답변을 비동기로 받는다.
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

        // 5. 답변이 여러개일 수 있으므로 첫번째 답변을 선택한다. (디폴트 = 1개)
        var choice = response.FirstChoice;

        // 6. 답변을 콘텍스트에 담는다.
        _message.Add(new Message(Role.Assistant, choice.Message));

        // 7. 결과값을 UI에 출력한다.
        _resultTextUI.text = choice.Message;

        // 8. TTS (Text To Speech)
        // 실시간 TTS가 필요하다면 한국 성우가 많은 타입캐스트 API 이용을 권장한다.
        var request = new SpeechRequest(
            input: choice.Message,
            model: Model.TTS_GPT_4o_Mini,
            voice: Voice.Sage);
        var speechClip = await api.AudioEndpoint.GetSpeechAsync(request);
        _audioSource.PlayOneShot(speechClip);

        // 9. 초기화를 한다.
        _promptTextField.text = string.Empty;
        _sendButton.interactable = true;
    }
}
