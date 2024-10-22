﻿using System.Collections;
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
                return "SC_Zone0_Final";

            case SceneState.ZoneOne:
                return "SC_Zone1_Final";

            case SceneState.ZoneTwo:
                return "SC_Zone2_Final";

            case SceneState.End:
                return "SC_End";
        }

        return string.Empty;
    }
}

public static class GameState
{
    #region Fields
    public static SceneState currentScene = SceneState.Tutorial;
    public static float[] speedrunTime = new float[3] { 0, 0, 0 };
    public static float[] deathCount = new float[3] { 0, 0, 0 };
    public static int[] maxCollectibles = new int[3] { 0, 0, 0 };
    public static int[] Collectibles = new int[3] { 0, 0, 0 };
    #endregion

    #region Properties
    public static float CurrentSpeedrunTime
    {
        get
        {
            try
            {
                speedrunTime[(int)currentScene] = Time.timeSinceLevelLoad;
            }
            catch (System.IndexOutOfRangeException)
            {

            }

            return speedrunTime[(int)currentScene];
        }
    }

    public static int CurrentCollectibles
    {
        get
        {
            return Collectibles[(int)currentScene];
        }

        set
        {
            Collectibles[(int)currentScene] = value;
        }
    }

    public static int CurrentMaxCollectibles
    {
        get
        {
            return maxCollectibles[(int)currentScene];
        }

        set
        {
            maxCollectibles[(int)currentScene] = value;
        }
    }

    public static float CurrentDeathCount
    {
        get
        {
            return deathCount[(int)currentScene];
        }

        set
        {
            deathCount[(int)currentScene] = value;
        }
    }
    #endregion

    public static void UpdateCurrentSpeedrunTime()
    {
        speedrunTime[(int)currentScene] = Time.timeSinceLevelLoad;
    }


}