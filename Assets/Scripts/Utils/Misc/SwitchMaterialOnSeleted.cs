using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SwitchMaterialOnSeleted : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _selectedObject = null;
    [Space]
    [SerializeField] private Material _selectedMaterial;
    [SerializeField] private Material _unselectMaterial;

    private TextMeshProUGUI _text;
    #endregion

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();

        if (_selectedObject == null)
        {
            _selectedObject = transform.parent.gameObject;
        }
    }

    void Update()
    {
        if (!EventSystem.current)
            return;

        if (EventSystem.current.currentSelectedGameObject == _selectedObject)
        {
            _text.fontSharedMaterial = _selectedMaterial;
        }
        else
        {
            _text.fontSharedMaterial = _unselectMaterial;
        }
    }
}
