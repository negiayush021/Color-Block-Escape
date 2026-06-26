using Solo.MOST_IN_ONE;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip[] clip;
    public AudioSource audioSource_Sound;
    public AudioSource audioSource_music;

    public bool can_play_sound = true;
    public bool can_vibrate = true;
    public bool musicPlaying = true;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void PlayClip(int index)
    {
        if (index >= 0 && index < clip.Length)
        {
            if (can_play_sound)
            {
                audioSource_Sound.PlayOneShot(clip[index]);
            }

        }
    }

    

    //Haptics down below
    public void LightImpactHaptic()
    {
        MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.LightImpact);
    }

    public void SuccessHaptic()
    {
        MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.Success);
    }

    public void HeavyImpactHaptic()
    {
        MOST_HapticFeedback.Generate(MOST_HapticFeedback.HapticTypes.HeavyImpact);
    }
}
