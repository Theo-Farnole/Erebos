using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosExtension
{
    public static void DrawRect(Vector3 v, Rect r)
    {
        v.z = 0;

        Gizmos.DrawLine(v + new Vector3(r.min.x, r.max.y), v + new Vector3(r.max.x, r.max.y));
        Gizmos.DrawLine(v + new Vector3(r.min.x, r.min.y), v + new Vector3(r.max.x, r.min.y));
        Gizmos.DrawLine(v + new Vector3(r.min.x, r.min.y), v + new Vector3(r.min.x, r.max.y));
        Gizmos.DrawLine(v + new Vector3(r.max.x, r.min.y), v + new Vector3(r.max.x, r.max.y));
    }

    public static void Draw2DLine(Vector2 from, Vector2 to)
    {
        Gizmos.DrawLine(from, to);
    }
}
