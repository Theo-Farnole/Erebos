using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class OptionsData
{
    public SystemLanguage language = Application.systemLanguage;

    public bool enableVSync = false;
    public bool isFullscreen = true;
    public FullScreenMode fullscreenMode = FullScreenMode.FullScreenWindow;

    public float soundGeneral = 1f;
    public float soundMusique = 1f;
    public float soundAmbiance = 1f;

    #region Properties
    public int FullScreenValue
    {
        get
        {
            if (!isFullscreen)
                return 0;

            switch (fullscreenMode)
            {
                case FullScreenMode.ExclusiveFullScreen:
                    return 1;

                case FullScreenMode.FullScreenWindow:
                    return 2;
            }

            return -1;
        }

        set
        {
            switch (value)
            {
                case 0:
                    isFullscreen = false;
                    break;

                case 1:
                    isFullscreen = true;
                    fullscreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;

                case 2:
                    isFullscreen = true;
                    fullscreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
        }
    }

    public int Language
    {
        get
        {
            if (language == SystemLanguage.French) return 1;
            else return 0; // return English
        }

        set
        {
            if (value == 1) language = SystemLanguage.French;
            else language = SystemLanguage.English;
        }
    }
    #endregion

    public void ApplySettings()
    {
        int vSyncCount = SaveSystem.OptionsData.enableVSync ? 2 : 0;
        QualitySettings.vSyncCount = vSyncCount;

        Screen.fullScreen = isFullscreen;
        Screen.fullScreenMode = fullscreenMode;
    }
}
