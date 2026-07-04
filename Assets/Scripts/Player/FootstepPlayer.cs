using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] AudioClips_Footstep;

    public void PlayFootstep()
    {
        if (AudioClips_Footstep.Length == 0)
        {
            return;
        }
        int randomIndex = Random.Range(0, AudioClips_Footstep.Length);
        SoundManager.Instance.PlaySFX(AudioClips_Footstep[randomIndex]);
    }
}
