using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class CustomDelay
{
    public static IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);

        task();
    }
}