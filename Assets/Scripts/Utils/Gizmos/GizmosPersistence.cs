using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosPersistence
{
    public static void DrawPersistentLine(Vector3 p1, Vector2 p2, float lifetime = 3f)
    {
        var gizmos = new GameObject().AddComponent<GizmosLine>();
        gizmos.p1 = p1;
        gizmos.p2 = p2;

        GameObject.Destroy(gizmos.gameObject, lifetime);
    }
}
