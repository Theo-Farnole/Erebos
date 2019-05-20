using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundGeneral { };
    public enum SoundMusic { };
    public enum SoundAmbiance { };

    public void PlaySoundGeneral(SoundGeneral sound)
    {
        float volume = SaveSystem.optionsData.soundGeneral;

        switch (sound)
        {
            default:
                break;
        }
    }

    public void PlaySoundMusic(SoundMusic sound)
    {
        float volume = SaveSystem.optionsData.soundMusique;

        switch (sound)
        {
            default:
                break;
        }
    }

    public void PlaySoundAmbiance(SoundAmbiance sound)
    {
        float volume = SaveSystem.optionsData.soundAmbiance;
        
        switch (sound)
        {
            default:
                break;
        }
    }
}
