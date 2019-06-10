using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public static class Initiate
{
    static bool areWeFading = false;

    // Create Fader object and assing the fade scripts and assign all the variables
    public static void Fade(AsyncOperation ao, Color col, float multiplier)
    {
        if (areWeFading)
        {
            var currentFader = GameObject.FindObjectOfType<Fader>().gameObject;

            Debug.Log("Already Fading: destroy old fader: " + currentFader);

            GameObject.Destroy(currentFader);
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

        areWeFading = true;
        scr.InitiateFader();
    }

    public static void DoneFading()
    {
        areWeFading = false;
    }
}
