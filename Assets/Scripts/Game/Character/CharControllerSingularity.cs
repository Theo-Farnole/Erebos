using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Form { Normal, Ethereal, Void };
public delegate void FormHandle(object sender, Form form);

public class CharControllerSingularity : MonoBehaviour
{
    #region Fields
    public static event FormHandle EventForm;

    [Header("Models Settings")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [Space]
    [SerializeField] private Material _matNormal;
    [SerializeField] private Material _matEthereal;
    [SerializeField] private Material _matVoid;

    public static Form form = Form.Normal;

    private bool _rightTriggerPressed = false;
    private bool _leftTriggerPressed = false;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        form = Form.Normal;
        UpdateForm();
    }

    void Update()
    {
        ManageInputs();
    }
    #endregion

    void ManageInputs()
    {
        // void form
        if (!_rightTriggerPressed && GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One) > 0f)
        {
            switch (form)
            {
                case Form.Normal:
                    form = Form.Void;
                    break;

                case Form.Ethereal:
                    form = Form.Void;
                    CharControllerManager.Instance.Attracted = false;
                    break;

                case Form.Void:
                    form = Form.Normal;
                    CharControllerManager.Instance.Attracted = false;
                    break;
            }

            EventForm?.Invoke(this, form);
            UpdateForm();
        }

        // ethereal form
        if (!_leftTriggerPressed && GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f)
        {
            switch (form)
            {
                case Form.Normal:
                    form = Form.Ethereal;
                    break;

                case Form.Void:
                    form = Form.Ethereal;
                    CharControllerManager.Instance.Attracted = false;
                    break;

                case Form.Ethereal:
                    form = Form.Normal;
                    CharControllerManager.Instance.Attracted = false;
                    break;
            }

            EventForm?.Invoke(this, form);
            UpdateForm();
        }

        _rightTriggerPressed = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One) > 0f ? true : false;
        _leftTriggerPressed = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f ? true : false;
    }

    void UpdateForm()
    {
        switch (form)
        {
            case Form.Normal:
                _meshRenderer.material = _matNormal;
                break;

            case Form.Ethereal:
                _meshRenderer.material = _matEthereal;
                break;

            case Form.Void:
                _meshRenderer.material = _matVoid;
                break;
        }
    }
}
