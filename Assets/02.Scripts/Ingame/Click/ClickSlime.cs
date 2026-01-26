using UnityEngine;

public class ClickSlime : MonoBehaviour, Clickable
{
    [SerializeField] private string _name;

    public bool OnClick(ClickInfo clickInfo)
    {
#if UNITY_EDITOR
        Debug.Log($"{_name}에게 먹이를 줬다!");
#endif

        // 클릭에 대한 여러 가지 피드백 필요

        // 한 클래스는 하나의 역할 / 책임만 가지자
        // ClickSlime: 슬라임(타겟)에 대한 중앙 관리자, 소통 창구 (객체지향 상호작용)
        // 1. 클릭 이펙트
        GetComponent<ScaleTweeningFeedback>().PlayTween();
        GetComponent<ColorFlashFeedback>().PlayFlash();
        // 2. 캐릭터 애니메이션 (있으면)
        // 3. 스케일 트위닝
        // 4. 데미지 플로팅
        // 5. 사운드 재생
        // 6. 화면 흔들림
        // 7. 재화 떨구기
        return true;
    }
}
