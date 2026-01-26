using UnityEngine;

public class Clicker : MonoBehaviour
{
    // 목적: 타겟을 클릭하면 클릭 판정이 되게 하고 싶다.
    public LayerMask ClickLayer;

    private void Update()
    {
        // 1. 마우스 클릭을 감지한다.
        if (Input.GetMouseButtonDown(0))
        {
            // 2. 마우스 좌표를 클릭한다.
            // 마우스 좌표계는 스크린 좌표계 (왼쪽 위가 0,0)
            Vector2 mousePosition = Input.mousePosition;
            Click(mousePosition);
        }
    }

    private void Click(Vector2 mousePosition)
    {
        // 마우스의 스크린 좌표계를 월드 좌표계로 바꿔줄 필요가 있다.
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // 3. 타겟이 맞다면 클릭한다.
        // 3-1. 마우스의 좌표가 타겟 위치와 비교했을 때 근처에 있는지 체크
        // 3-2. 마우스 좌표로 가상의 레이저를 쏴서, 그 레이저가 타겟과 충돌했는지 체크 (보통 이걸로 함)
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit == true)
        {
            IClickable clickable = hit.collider.GetComponent<IClickable>();
            // 누가 클릭했는지                (ManualClick, AutoClick)
            // 어느 정도의 강도로 클릭했는지  (int, float)

            ClickInfo clickInfo = new ClickInfo
            {
                ClickType = EClickType.Manual,
                Damage = GameManager.Instance.ManualDamage,
                Position = hit.point
            };

            clickable?.OnClick(clickInfo);
        }

        // 4. 게임 오브젝트 설정
        // - ClickTarget (혹은 타겟명)이라는 Layer 추가
        // - Inspactor에서 Layer 설정
    }
}
