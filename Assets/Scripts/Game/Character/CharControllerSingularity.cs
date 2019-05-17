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

    private bool _rightTriggerPressed = false;
    private bool _leftTriggerPressed = false;

    private Form _form = Form.Normal;
    #endregion

    #region Properties
    public Form Form
    {
        get
        {
            return _form;
        }

        set
        {
            _form = value;
            UpdateForm();
            EventForm?.Invoke(this, Form);
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        Form = Form.Normal;
        UpdateForm();

        // on death, return to normal form
        DeathHandle d = new DeathHandle((object sender) =>
        {
            Form = Form.Normal;
        });
        CharDeath.EventDeath += d;
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
            switch (Form)
            {
                case Form.Normal:
                    Form = Form.Void;
                    break;

                case Form.Ethereal:
                    Form = Form.Void;
                    CharControllerManager.Instance.Attracted = false;
                    break;

                case Form.Void:
                    Form = Form.Normal;
                    CharControllerManager.Instance.Attracted = false;
                    break;
            }
        }

        // ethereal form
        if (!_leftTriggerPressed && GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f)
        {
            switch (Form)
            {
                case Form.Normal:
                    Form = Form.Ethereal;
                    break;

                case Form.Void:
                    Form = Form.Ethereal;
                    CharControllerManager.Instance.Attracted = false;
                    break;

                case Form.Ethereal:
                    Form = Form.Normal;
                    CharControllerManager.Instance.Attracted = false;
                    break;
            }
        }

        _rightTriggerPressed = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One) > 0f ? true : false;
        _leftTriggerPressed = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f ? true : false;
    }

    void UpdateForm()
    {
        switch (Form)
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
