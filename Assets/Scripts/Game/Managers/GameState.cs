using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneState { Tutorial, ZoneOne, ZoneTwo, End };

public static class SceneStateExtension
{
    public static string ToScene(this SceneState s)
    {
        switch (s)
        {
            case SceneState.Tutorial:
                return "tutorial";

            case SceneState.ZoneOne:
                return "Zone1";

            case SceneState.ZoneTwo:
                return "Zone2";

            case SceneState.End:
                return "End";
        }

        return string.Empty;
    }
}

public static class GameState
{
    #region Fields
    public static SceneState state = SceneState.Tutorial;
    public static float[] speedrunTime = new float[3] { 0, 0, 0 };
    public static float[] deathCount = new float[3] { 0, 0, 0 };
    private static int[] _maxCollectibles = new int[3] { 0, 0, 0 };
    #endregion

    #region Properties
    public static float CurrentSpeedrunTime
    {
        get
        {
            return speedrunTime[(int)state];
        }

        set
        {
            speedrunTime[(int)state] = Time.timeSinceLevelLoad;
        }
    }

    public static int CurrentMaxCollectibles
    {
        get
        {
            return _maxCollectibles[(int)state];
        }

        set
        {
            _maxCollectibles[(int)state] = value;
        }
    }


    public static float CurrentDeathCount
    {
        get
        {
            return deathCount[(int)state];
        }

        set
        {
            deathCount[(int)state] = value;
        }
    }
    #endregion
}