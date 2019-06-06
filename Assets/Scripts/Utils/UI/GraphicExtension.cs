using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType { FadeIn, FadeOut }

public static class GraphicExtension
{
    public static void Fade(this Graphic g, FadeType fadeType, float timeToFadout)
    {
        // set alpha transparency
        var color = g.color;

        switch (fadeType)
        {
            case FadeType.FadeIn:
                color.a = 0;
                break;

            case FadeType.FadeOut:
                color.a = 1;
                break;
        }

        g.color = color;

        // start coroutine
        g.GetComponent<MonoBehaviour>().StartCoroutine(FadeCoroutine(g, fadeType, timeToFadout));
    }

    public static IEnumerator FadeCoroutine(Graphic g, FadeType fadeType, float timeToFadout)
    {
        Color color = g.color;
        float startingTime = Time.unscaledTime;

        do
        {
            float deltaTime = Time.unscaledTime - startingTime;
            float newAlpha = 0;

            if (fadeType == FadeType.FadeIn) newAlpha = Mathf.Lerp(0, 1, deltaTime / timeToFadout);
            if (fadeType == FadeType.FadeOut) newAlpha = Mathf.Lerp(1, 0, deltaTime / timeToFadout);

            color.a = newAlpha;
            g.color = color;

            yield return new WaitForEndOfFrame();

        } while (color.a > 0);
    }
}
