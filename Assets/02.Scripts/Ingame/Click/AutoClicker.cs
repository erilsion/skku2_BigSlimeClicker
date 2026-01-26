using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    // 역할: 정해진 시간 간격마다 Clickable한 대상을 히트한다.
    [SerializeField] private float _interval;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_timer >= _interval)  // 1. 시간 간격마다.
        {
            _timer = 0;

            // 2. Clickable 게임 오브젝트를 모두 찾아온다.
            GameObject[] clickables = GameObject.FindGameObjectsWithTag("Clickable");
            foreach (GameObject clickable in clickables)
            {
                // 3. 클릭한다.
                IClickable clickableScript = clickable.GetComponent<IClickable>();
                if (clickableScript != null)
                {
                    ClickInfo clickInfo = new ClickInfo
                    {
                        ClickType = EClickType.Auto,
                        Damage = GameManager.Instance.AutoDamage
                    };
                    clickableScript.OnClick(clickInfo);
                }
            }
        }
    }
}
