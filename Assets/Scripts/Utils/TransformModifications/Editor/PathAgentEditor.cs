using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathAgent))]
public class PathAgentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PathAgent myScript = (PathAgent)target;

        if (GUILayout.Button("Reverse path"))
        {
            myScript.ReversePath();
        }
    }
}
