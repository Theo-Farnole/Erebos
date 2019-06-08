using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FadeType { FadeIn, FadeOut }

public static class GraphicExtension
{
    /// <summary>
    /// Fade graphic component.
    /// </summary>
    public static void Fade(this Graphic g, FadeType fadeType, float timeToFadout)
    {
        g.StopAllCoroutines();

        Debug.Log(g.transform.name + " " + fadeType + " with a duration of " + timeToFadout);

        new Timer(g, timeToFadout, (float f) =>
        {
            var color = g.color;

            switch (fadeType)
            {
                case FadeType.FadeIn:
                    color.a = f;
                    break;

                case FadeType.FadeOut:
                    color.a = 1 - f;
                    break;
            }

            g.color = color;
        });
    }

    public static void SetTransparency(this Graphic g, float alpha)
    {
        Color c = g.color;
        c.a = alpha;
        g.color = c;
    }
}
