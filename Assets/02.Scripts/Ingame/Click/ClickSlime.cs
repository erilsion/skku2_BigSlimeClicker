using UnityEngine;

public class ClickSlime : MonoBehaviour, IClickable
{
    [SerializeField] private string _name;

    public Vector2 Position => transform.position;

    private void OnEnable()
    {
        ClickableRegistry.Register(this);
    }

    private void OnDisable()
    {
        ClickableRegistry.Unregister(this);
    }

    public bool OnClick(ClickInfo clickInfo)
    {
#if UNITY_EDITOR
        Debug.Log($"{_name}에게 먹이를 줬다!");
#endif

        // 1. 클릭 피드백
        var feedbacks = GetComponentsInChildren<IFeedback>();
        foreach (var feedback in feedbacks)
        {
            feedback.Play(clickInfo);
        }

        // 2. 애니메이션
        // 3. 스케일 트윈
        // 4. 데미지 플로팅
        // 5. 사운드
        // 6. 화면 흔들림
        // 7. 재화 처리 / 체력 감소는 다른 컴포넌트에 위임 가능

        return true;
    }
}
