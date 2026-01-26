using UnityEngine;

public class DamageFloaterFeedback : MonoBehaviour, IFeedback
{
    [Header("Pool")]
    [SerializeField] private DamageFloaterPool _pool;

    [Header("Spawn Offset")]
    [SerializeField] private Vector2 _randomOffset = new(0.25f, 0.15f);

    private bool _init;

    private void Awake()
    {
        // 풀 루트는 이 오브젝트 밑으로 정리한다.
        if (_pool != null)
        {
            _pool.Initialize(transform);
            _init = true;
        }
    }

    public void Play(ClickInfo clickInfo)
    {
        if (!_init && _pool != null) _pool.Initialize(transform);

        Vector2 offset = new(
            Random.Range(-_randomOffset.x, _randomOffset.x),
            Random.Range(-_randomOffset.y, _randomOffset.y)
        );

        Vector3 position = (Vector3)(clickInfo.Position + offset);

        if (_pool == null)
        {
            return;
        }

        var floater = _pool.Get(position);

        if (floater != null)
        {
            floater.Play(clickInfo.Damage);
        }
    }
}
