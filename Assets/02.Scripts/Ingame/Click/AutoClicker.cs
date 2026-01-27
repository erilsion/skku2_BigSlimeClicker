using UnityEngine;

public class AutoClicker : MonoBehaviour
{
    // 역할: 정해진 시간 간격마다 Clickable한 대상을 히트한다.
    [SerializeField] private float _interval = 3f;
    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        // 1. 시간 간격마다 발동한다.
        if (_timer < _interval)
        {
            return;
        }
        _timer -= _interval;

        // 2. Clickable 게임 오브젝트를 모두 찾아온다.
        var clickables = ClickableRegistry.List;
        if (clickables.Count == 0) 
        {
            return;
        }
        double damage = DamageCalculation.Instance.GetAutoDamage();

        ClickInfo clickInfo = new ClickInfo
        {
            ClickType = EClickType.Auto,
            Damage = damage
        };

        // 3. 클릭한다.
        for (int i = 0; i < clickables.Count; i++)
        {
            var clickable = clickables[i];
            if (clickable == null)
            {
                continue;
            }

            clickInfo.Position = clickable.Position;
            clickable.OnClick(clickInfo);
        }
    }
}
