using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    /// <summary>
    /// Constructor of Timer
    /// </summary>
    /// <param name="ownerOfCoroutine">MonoBehaviour which will start the Coroutine</param>
    /// <param name="duration">Duration of the timer</param>
    /// <param name="task">float parameter is percent of progress of timer. Clamped between 0 and 1.</param>
    public Timer(MonoBehaviour ownerOfCoroutine, float duration, Action<float> task)
    {
        var monoBehaviour = ownerOfCoroutine.GetComponent<MonoBehaviour>();
        monoBehaviour.StartCoroutine(Coroutine(task, duration));
    }

    IEnumerator Coroutine(Action<float> task, float duration)
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
