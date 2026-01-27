using UnityEngine;
using Lean.Pool;

public class DamageFloaterSpawner : MonoBehaviour
{
    public static DamageFloaterSpawner Instance { get; private set; }

    [SerializeField] private LeanGameObjectPool _pool;

    [Header("스폰 간격")]
    [SerializeField] private Vector2 _randomOffset = new(0.25f, 0.15f);

    private void Awake()
    {
        Instance = this;
        _pool = GetComponent<LeanGameObjectPool>();
    }

    public void ShowDamage(ClickInfo clickInfo)
    {
        // 글씨 스폰 랜덤 간격을 설정한다.
        Vector2 offset = new(
            Random.Range(-_randomOffset.x, _randomOffset.x),
            Random.Range(-_randomOffset.y, _randomOffset.y)
        );
        Vector3 position = (Vector3)(clickInfo.Position + offset);

        // 1. 풀로부터 DamageFloater를 가져온다.
        GameObject floaterObject = _pool.Spawn(position, Quaternion.identity);
        DamageFloater floater = floaterObject.GetComponent<DamageFloater>();

        // 2. 클릭한 위치에 생성한다.
        floater.Show(clickInfo.Damage);
    }
}
