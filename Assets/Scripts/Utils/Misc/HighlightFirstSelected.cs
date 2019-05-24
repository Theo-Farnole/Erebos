using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventSystem))]
public class HighlightFirstSelected : MonoBehaviour
{
    private void Awake()
    {
        var firstSelected = GetComponent<EventSystem>().firstSelectedGameObject.GetComponent<Selectable>();
        firstSelected.targetGraphic.color = firstSelected.colors.selectedColor;
        
    }
}
