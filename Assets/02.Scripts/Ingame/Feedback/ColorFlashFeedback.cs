using UnityEngine;
using System.Collections;

public class ColorFlashFeedback : MonoBehaviour, IFeedback
{
    private Coroutine _coroutine;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _flashColor;

    private float _flashIntensity = 0.2f;

    public void Play(ClickInfo clickInfo)
    {
        PlayFlash();
    }

    private void PlayFlash()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }

        _coroutine = StartCoroutine(PlayFlash_Coroutine());
    }

    private IEnumerator PlayFlash_Coroutine()
    {
        _spriteRenderer.color = _flashColor;

        yield return new WaitForSeconds(_flashIntensity);

        _spriteRenderer.color = Color.white;
    }
}
