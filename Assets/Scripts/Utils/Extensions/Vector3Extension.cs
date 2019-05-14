using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    public static void Clamp(this Vector3 v, float min, float max)
    {
        v.x = Mathf.Clamp(v.x, min, max);
        v.y = Mathf.Clamp(v.y, min, max);
        v.z = Mathf.Clamp(v.z, min, max);
    }

    // note: this method can be OPTIMIZED if need
    public static void Clamp(this Vector3 v1, Vector3 v2)
    {
        v1.x = Mathf.Clamp(v1.x, -Mathf.Abs(v2.x), Mathf.Abs(v2.x));
        v1.y = Mathf.Clamp(v1.y, -Mathf.Abs(v2.y), Mathf.Abs(v2.y));
        v1.z = Mathf.Clamp(v1.z, -Mathf.Abs(v2.z), Mathf.Abs(v2.z));
    }
}
