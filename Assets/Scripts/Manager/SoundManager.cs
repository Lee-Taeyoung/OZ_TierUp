using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private float _sfxVolume = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _sfxSource.volume = _sfxVolume;
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1f)
    {
        if (_sfxSource == null || clip == null)
        {
            return;
        }
        
        _sfxSource.PlayOneShot(clip, _sfxVolume * volumeScale);
    }
}