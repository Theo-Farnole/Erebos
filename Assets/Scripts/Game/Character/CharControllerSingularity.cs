using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharControllerSingularity : MonoBehaviour
{
    #region Fields

    [Header("Models Settings")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [Space]
    [SerializeField] private Material _matNormal;
    [SerializeField] private Material _matEthereal;
    [SerializeField] private Material _matVoid;

    public enum Form { Normal, Ethereal, Void };
    public static Form form = Form.Normal;
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
        if (GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One) > 0f)
        {
            form = Form.Void;
            UpdateForm();
        }

        // ethereal form
        if (GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f)
        {
            form = Form.Ethereal;
            UpdateForm();
        }
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
