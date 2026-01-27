using UnityEngine;
using TMPro;
using System.Collections;
using Lean.Pool;

public class DamageFloater : MonoBehaviour
{
    [Header("텍스트")]
    [SerializeField] private TMP_Text _text;

    [Header("모션")]
    [SerializeField] private Vector3 _moveVelocity = new(0f, 1.2f, 0f);
    [SerializeField] private float _lifeTime = 0.6f;
    [SerializeField] private float _scalePunch = 1.15f;
    [SerializeField] private float _scaleReturnSpeed = 12f;

    private Vector3 _baseScale;
    private Coroutine _showCoroutine;

    public void Show(double damage)
    {
        gameObject.SetActive(true);

        if (_text != null)
        {
            _text.text = damage.ToString();
        }

        // 초기 스케일 값을 세팅한다.
        _baseScale = transform.localScale;
        transform.localScale = _baseScale * _scalePunch;

        if (_text != null)
        {
            var color = _text.color;
            color.a = 1f;
            _text.color = color;
        }

        // 이미 돌고 있던 코루틴이 있으면 끊고 새로 시작한다.
        if (_showCoroutine != null)
        {
            StopCoroutine(_showCoroutine);
        }
        _showCoroutine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        float timer = 0f;

        while (timer < _lifeTime)
        {
            timer += Time.deltaTime;

            transform.position += _moveVelocity * Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, _baseScale, _scaleReturnSpeed * Time.deltaTime);

            // 효과를 위해 알파값 변화를 준다.
            if (_text != null)
            {
                float a = Mathf.Lerp(1f, 0f, timer / _lifeTime);
                var c = _text.color;
                c.a = a;
                _text.color = c;
            }

            yield return null;
        }

        _showCoroutine = null;
        LeanPool.Despawn(gameObject);
    }

    private void OnDisable()
    {
        // 풀로 돌아갔다가 다시 나올 때 상태 꼬임을 방지한다.
        if (_showCoroutine != null)
        {
            StopCoroutine(_showCoroutine);
            _showCoroutine = null;
        }
    }
}
