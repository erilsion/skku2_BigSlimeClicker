using UnityEngine;
using TMPro;

public class DamageFloater : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TMP_Text _text;

    [Header("Motion")]
    [SerializeField] private Vector3 _moveVelocity = new(0f, 1.2f, 0f);
    [SerializeField] private float _lifeTime = 0.6f;
    [SerializeField] private float _scalePunch = 1.15f;
    [SerializeField] private float _scaleReturnSpeed = 12f;

    private float _t;
    private Vector3 _baseScale;

    private DamageFloaterPool _pool;

    public void BindPool(DamageFloaterPool pool) => _pool = pool;

    public void Play(int damage)
    {
        if (_text != null) _text.text = damage.ToString();

        _t = 0f;
        _baseScale = transform.localScale;
        transform.localScale = _baseScale * _scalePunch;

        // 알파 초기화
        if (_text != null)
        {
            var c = _text.color;
            c.a = 1f;
            _text.color = c;
        }
    }

    private void Update()
    {
        _t += Time.deltaTime;

        transform.position += _moveVelocity * Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, _baseScale, _scaleReturnSpeed * Time.deltaTime);

        if (_text != null)
        {
            float a = Mathf.Lerp(1f, 0f, _t / _lifeTime);
            var c = _text.color;
            c.a = a;
            _text.color = c;
        }

        if (_t >= _lifeTime)
        {
            if (_pool != null) _pool.Release(this);
            else gameObject.SetActive(false);
        }
    }
}
