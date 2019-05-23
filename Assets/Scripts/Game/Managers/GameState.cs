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
    public static SceneState state = SceneState.Tutorial;
    public static float[] speedrunTime = new float[3] { 0, 0, 0 };
    public static float[] deathCount = new float[3] { 0, 0, 0 };

    public static void SaveSpeedRunTime()
    {
        speedrunTime[(int)state] = Time.timeSinceLevelLoad;
    }

    public static void OneMoreDeath()
    {
        deathCount[(int)state]++;
    }
}