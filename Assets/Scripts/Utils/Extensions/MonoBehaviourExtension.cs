using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtension
{
    /// <summary>
    /// Execute task after time.
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="time"></param>
    /// <param name="task"></param>
    public static Coroutine ExecuteAfterTime(this MonoBehaviour mono, float time, Action task)
    {
        return mono.StartCoroutine(Coroutine(time, task));
    }

    /// <summary>
    /// Called from ExecuteAfterTime.
    /// </summary>
    static IEnumerator Coroutine(float time, Action task)
    {
        yield return new WaitForSeconds(time);

        task();
    }
}
