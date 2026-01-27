using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("쉐이크 옵션")]
    private float _duration = 0.12f;
    private float _minStrength = 0.02f;
    private float _maxStrength = 0.1f;
    private int _vibration = 6;
    private float _randomness = 90f;

    private Vector3 _originLocalPosition;
    private Tween _tween;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _originLocalPosition = transform.localPosition;
    }

    public void Shake(float power01)
    {
        power01 = Mathf.Clamp01(power01);

        float strength = Mathf.Lerp(_minStrength, _maxStrength, power01);

        _tween?.Kill();
        transform.localPosition = _originLocalPosition;

        _tween = transform.DOShakePosition(
            _duration,
            new Vector3(strength, strength, 0f),
            _vibration,
            _randomness,
            false,
            true
        );
    }
}
