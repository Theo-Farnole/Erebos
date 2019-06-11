using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SelectorEditor
{
    [MenuItem("GameObject/UI Extensions/Selector", false, 1)]
    public static void Foo(MenuCommand menuCommand)
    {
        var newGameObject = CreateSelector();

        GameObjectUtility.SetParentAndAlign(newGameObject, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(newGameObject, "Create " + newGameObject.name);
        Selection.activeObject = newGameObject;
        SceneView.FrameLastActiveSceneViewWithLock();
    }

    private static GameObject CreateSelector()
    {
        var go = new GameObject();

        go.AddComponent<RectTransform>();
        go.AddComponent<Selector>();
        go.name = "Selector";

        var label = new GameObject();
        label.AddComponent<RectTransform>();
        label.AddComponent<TextMeshProUGUI>();
        label.name = "Label";

        // parenting
        label.transform.parent = go.transform;

        return go;
    }
}
