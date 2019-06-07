using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundGeneral
{
    Jump,
    Collectible,
    Dash,
    FormToBlack,
    FormToWhite,
    Checkpoint,
    Death,
    Respawn,
    WallGrab,
    WallJump,
};

public enum SoundUI
{
    Button
}

public enum SoundMusic
{
};
public enum SoundAmbiance
{
};

// NOTE:
// Project convention: Never call PlaySound from event!
public class AudioManager : Singleton<AudioManager>
{
    #region Class
    [System.Serializable]
    public class GeneralSounds
    {
        #region Fields
        [SerializeField] private AudioSource _collectible;
        [Header("Mouvements")]
        [SerializeField] private AudioSource _jump;
        [SerializeField] private AudioSource _dash;
        [Space]
        [SerializeField] private AudioSource _wallJump;
        [SerializeField] private AudioSource _wallGrab;
        [Header("Forms")]
        [SerializeField] private AudioSource _formToBlack;
        [SerializeField] private AudioSource _formToWhite;
        [Header("Checkpoints & Death")]
        [SerializeField] private AudioSource _checkpoint;
        [Space]
        [SerializeField] private AudioSource _death;
        [SerializeField] private AudioSource _respawn;
        [Space]
        [SerializeField] private List<AudioSource> _footsteps = new List<AudioSource>();

        private Dictionary<SoundGeneral, AudioSource> _sounds = null;
        #endregion

        #region Properties
        public List<AudioSource> Footsteps { get => _footsteps; }
        public Dictionary<SoundGeneral, AudioSource> Sounds
        {
            get
            {
                if (_sounds == null)
                {
                    _sounds = new Dictionary<SoundGeneral, AudioSource>
                    {
                        { SoundGeneral.Jump, _jump },
                        { SoundGeneral.Collectible, _collectible },
                        { SoundGeneral.Dash, _dash },
                        { SoundGeneral.FormToBlack, _formToBlack},
                        { SoundGeneral.FormToWhite, _formToWhite },
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
        #endregion
    }

    [System.Serializable]
    public class UISounds
    {
        #region Fields
        [SerializeField] private AudioSource _button;

        private Dictionary<SoundUI, AudioSource> _sounds = null;
        #endregion

        #region Properties
        public Dictionary<SoundUI, AudioSource> Sounds
        {
            get
            {
                if (_sounds == null)
                {
                    _sounds = new Dictionary<SoundUI, AudioSource>
                    {
                        { SoundUI.Button, _button}
                    };
                }

                return _sounds;
            }
        }
        #endregion
    }
    #endregion

    #region Fields
    [SerializeField] private GeneralSounds _generalSounds;
    [SerializeField] private UISounds _uiSounds;
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

    public void PlayFootsteps()
    {
        int random = Random.Range(0, _generalSounds.Footsteps.Count);

        _generalSounds.Footsteps[random].Play();
    }

    public void PlaySoundUI(SoundUI sound)
    {
        float volume = SaveSystem.OptionsData.soundGeneral;

        if (_uiSounds.Sounds[sound] != null)
        {
            _uiSounds.Sounds[sound].Play();
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
