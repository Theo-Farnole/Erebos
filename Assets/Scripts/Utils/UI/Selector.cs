using Erebos.Inputs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Selector : Selectable
{
    #region Fields

    [Header("Selector Settings")]
    [SerializeField] private List<string> _options = new List<string>();
    [SerializeField] private TextMeshProUGUI _label;
    [Space]
    public UnityEvent onValueChanged;

    private int _value = 0;

    private bool _isHorizontalPressed = false;
    private bool _isSelected = false;
    #endregion

    #region Fields
    public int Value
    {
        get
        {
            return _value;
        }

        set
        {
            // clamp value
            if (value < 0)
            {
                value = _options.Count - 1;
            }
            else if (value >= _options.Count)
            {
                value = 0;
            }

            // Invoke Event
            if (_value != value)
            {
                onValueChanged?.Invoke();
            }

            _value = value;

            UpdateLabel();
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    protected override void Awake()
    {
        onValueChanged = new UnityEvent();
        Value = 0;
    }

    void Update()
    {
        if (_isSelected)
        {
            Selected();
        }
    }
    #endregion

    #region Selectable Callbacks
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        _isSelected = true;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        _isSelected = false;
    }
    #endregion

    void Selected()
    {
        float horizontal = InputProxy.Character.Horizontal;
        bool isHorizontalDown = (horizontal != 0) && (_isHorizontalPressed == false);

        if (isHorizontalDown)
        {
            Value += (int)Mathf.Sign(horizontal);
        }

        _isHorizontalPressed = (horizontal != 0);
    }

    void UpdateLabel()
    {

        if (_options.Count > 1)
        {
        _label.text = _options[Value];
        }
    }
}
