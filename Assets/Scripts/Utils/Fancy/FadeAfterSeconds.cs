using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAfterSeconds : MonoBehaviour
{
    [SerializeField] private float _timeBeforeFade = 1;
    [Space]
    [SerializeField] private FadeType _fadeType = FadeType.FadeIn;
    [SerializeField] private float _fadeDuration = 1;


    void Start()
    {
        Graphic graphic = GetComponent<Graphic>();

        // set alpha to 0
        if (_fadeType == FadeType.FadeIn)
        {
            var color = graphic.color;
            color.a = 0;
            graphic.color = color;
        }

        if (graphic != null)
        {
            this.ExecuteAfterTime(_timeBeforeFade, () => graphic.Fade(_fadeType, _fadeDuration));
        }
    }
}
