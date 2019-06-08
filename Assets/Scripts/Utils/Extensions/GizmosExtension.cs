using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosExtension
{
    /// <summary>
    /// Draw Rect in with Gizmos line.
    /// </summary>
    public static void DrawRect(Rect r)
    {
        DrawRect(r, Vector3.zero);
    }

    /// <summary>
    /// Draw Rect in with Gizmos line.
    /// </summary>
    public static void DrawRect(Rect r, Vector3 offset)
    {
        offset.z = 0;

        Gizmos.DrawLine(offset + new Vector3(r.min.x, r.max.y), offset + new Vector3(r.max.x, r.max.y));
        Gizmos.DrawLine(offset + new Vector3(r.min.x, r.min.y), offset + new Vector3(r.max.x, r.min.y));
        Gizmos.DrawLine(offset + new Vector3(r.min.x, r.min.y), offset + new Vector3(r.min.x, r.max.y));
        Gizmos.DrawLine(offset + new Vector3(r.max.x, r.min.y), offset + new Vector3(r.max.x, r.max.y));
    }

    /// <summary>
    /// Draw gizmos line without Z axis.
    /// </summary>
    public static void Draw2DLine(Vector2 from, Vector2 to)
    {
        Gizmos.DrawLine(from, to);
    }
}
