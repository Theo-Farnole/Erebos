using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class Initiate
{
    static bool areWeFading = false;

    public static bool AreWeFading { get => areWeFading; }

    public static void Fade(string sceneName, Color col, float multiplier)
    {
        Fade(null, sceneName, col, multiplier);
    }

    //Create Fader object and assing the fade scripts and assign all the variables
    public static void Fade(AsyncOperation ao, Color col, float multiplier)
    {
        Fade(ao, string.Empty, col, multiplier);
    }

    public static void DoneFading()
    {
        areWeFading = false;
    }

    public static void StopFading()
    {
        var faders = GameObject.FindObjectsOfType<Fader>();

        foreach (var f in faders)
        {
            GameObject.Destroy(f.gameObject);
        }

        DoneFading();
    }

    private static void Fade(AsyncOperation ao, string sceneName, Color col, float multiplier)
    {
        StopFading();

        GameObject init = new GameObject();
        init.name = "Fader";
        Canvas myCanvas = init.AddComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        init.AddComponent<Fader>();
        init.AddComponent<CanvasGroup>();
        init.AddComponent<Image>();

        Fader scr = init.GetComponent<Fader>();
        scr.fadeDamp = multiplier;
        scr.fadeColor = col;
        scr.start = true;

        if (ao != null) scr.fadeAsyncOperation = ao;
        if (sceneName != string.Empty) scr.fadeScene = sceneName;


        areWeFading = true;
        scr.InitiateFader();
    }
}
