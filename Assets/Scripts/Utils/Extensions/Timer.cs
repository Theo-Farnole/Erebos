using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public Timer(GameObject ownerOfCoroutine, float duration, Action<float> task)
    {
        ownerOfCoroutine.GetComponent<MonoBehaviour>().StartCoroutine(Coroutine(task, duration));
    }


    public IEnumerator Coroutine(Action<float> task, float duration)
    {
        float startingTime = Time.unscaledTime;
        float time = 0;

        do
        {
            float deltaTime = Time.unscaledTime - startingTime;

            time = Mathf.Lerp(0, 1, deltaTime / duration);
            task(time);

            yield return new WaitForEndOfFrame();

        } while (time < 1);
    }
}
