using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundGeneral
{
    Jump,
    Footstep,
    Collectible,
    Dash,
    FormChange,
    Checkpoint,
    Death,
    Respawn,
    WallGrab,
    WallJump,
};

public enum SoundMusic
{
};
public enum SoundAmbiance
{
};

public class AudioManager : Singleton<AudioManager>
{
    #region Class
    [System.Serializable]
    public class GeneralSounds
    {
        [SerializeField] private AudioSource _jump;
        [SerializeField] private AudioSource _footstep;
        [SerializeField] private AudioSource _collectible;
        [SerializeField] private AudioSource _dash;
        [SerializeField] private AudioSource _formChange;
        [SerializeField] private AudioSource _checkpoint;
        [SerializeField] private AudioSource _death;
        [SerializeField] private AudioSource _respawn;
        [SerializeField] private AudioSource _wallGrab;
        [SerializeField] private AudioSource _wallJump;

        private Dictionary<SoundGeneral, AudioSource> _sounds = null;

        public Dictionary<SoundGeneral, AudioSource> Sounds
        {
            get
            {
                if (_sounds == null)
                {
                    _sounds = new Dictionary<SoundGeneral, AudioSource>
                    {
                        { SoundGeneral.Jump, _jump },
                        { SoundGeneral.Footstep, _footstep },
                        { SoundGeneral.Collectible, _collectible },
                        { SoundGeneral.Dash, _dash },
                        { SoundGeneral.FormChange, _formChange },
                        { SoundGeneral.Checkpoint, _checkpoint },
                        { SoundGeneral.Death, _death },
                        { SoundGeneral.Respawn, _respawn },
                        { SoundGeneral.WallGrab, _wallGrab },
                        { SoundGeneral.WallJump, _wallJump }
                    };
                }

                return _sounds;
            }
        }
    }
    #endregion

    #region Fields
    [SerializeField] private GeneralSounds _generalSounds;
    #endregion

    public void PlaySoundGeneral(SoundGeneral sound)
    {
        float volume = SaveSystem.OptionsData.soundGeneral;

        if (_generalSounds.Sounds[sound] != null)
        {
            _generalSounds.Sounds[sound].Play();
        }
        else
        {
            Debug.LogWarning(sound.ToString() + " isn't set in AudioManager!");
        }
    }

    public void PlaySoundMusic(SoundMusic sound)
    {
        float volume = SaveSystem.OptionsData.soundMusique;

        switch (sound)
        {
            default:
                break;
        }
    }

    public void PlaySoundAmbiance(SoundAmbiance sound)
    {
        float volume = SaveSystem.OptionsData.soundAmbiance;

        switch (sound)
        {
            default:
                break;
        }
    }
}
