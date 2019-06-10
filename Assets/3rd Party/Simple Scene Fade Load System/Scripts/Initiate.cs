using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public static class Initiate
{
    // Create Fader object and assing the fade scripts and assign all the variables
    public static void Fade(AsyncOperation ao, Color col, float multiplier)
    {
        var faders = GameObject.FindObjectsOfType<Fader>();

        foreach (var f in faders)
        {
            GameObject.Destroy(f.gameObject);
        }


        GameObject init = new GameObject
        {
            name = "Fader"
        };

        Canvas myCanvas = init.AddComponent<Canvas>();
        myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        init.AddComponent<Fader>();
        init.AddComponent<CanvasGroup>();
        init.AddComponent<Image>();

        Fader scr = init.GetComponent<Fader>();
        scr.fadeDamp = multiplier;
        scr.ao = ao;
        scr.fadeColor = col;

        scr.InitiateFader();
    }
}
