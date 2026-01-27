using UnityEngine;

public class SoundFeedback : MonoBehaviour, IFeedback
{
    [SerializeField] private AudioSource _audio;

    private float _lowPitch = 0.8f;
    private float _highPitch = 1.2f;

    public void Play(ClickInfo clickInfo)
    {
        if(clickInfo.ClickType == EClickType.Auto)
        {
            return;
        }
        if (_audio != null)
        {
            _audio.pitch = Random.Range(_lowPitch, _highPitch);
            _audio.Play();
        }
    }
}
