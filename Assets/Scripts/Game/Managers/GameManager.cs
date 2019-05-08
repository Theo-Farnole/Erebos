using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    void Awake()
    {
        SaveSystem.Load();

        ApplySavedSettings();
    }

    public void ApplySavedSettings()
    {
        int vSyncCount = SaveSystem.optionsData.enableVSync ? 2 : 0;
        QualitySettings.vSyncCount = vSyncCount;
    }
}
