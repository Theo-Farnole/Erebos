using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Form { Normal, Ethereal, Void };
public delegate void FormHandle(object sender, Form form);

public class CharControllerSingularity : MonoBehaviour
{
    #region Fields
    #region Publics Fields
    public static event FormHandle EventForm;

    [HideInInspector] public bool canGotoVoid = true;
    [HideInInspector] public bool canGotoEtheral = true;
    #endregion

    #region Serialize Fields
    [Header("Models Settings")]
    [SerializeField] private MeshRenderer _meshRenderer;
    [Space]
    [SerializeField] private Material _matNormal;
    [SerializeField] private Material _matEthereal;
    [SerializeField] private Material _matVoid;
    #endregion

    #region Private Fields
    private bool _rightTriggerPressed = false;
    private bool _leftTriggerPressed = false;

    private Form _form = Form.Normal;
    #endregion
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
            EventForm?.Invoke(this, _form);

            Debug.Log("EventForm invoke");
        }
    }
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        bool isInTutorial = FindObjectsOfType<AbstractSingularity>().Length == 0 && FindObjectsOfType<Eclatos>().Length == 0;
        bool isInFirstZone = FindObjectsOfType<TutorialFeather>().Length > 0;

        canGotoVoid = !(isInTutorial || isInFirstZone);
        canGotoEtheral = !(isInTutorial || isInFirstZone);

        Form = Form.Normal;

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

    void OnDestroy()
    {
        EventForm = null;
    }
    #endregion

    void ManageInputs()
    {
        // void form
        if (canGotoVoid && !_rightTriggerPressed && GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One) > 0f)
        {
            switch (Form)
            {
                case Form.Normal:
                    Form = Form.Void;
                    break;

                case Form.Ethereal:
                    Form = Form.Void;
                    break;

                case Form.Void:
                    Form = Form.Normal;
                    break;
            }
        }

        // ethereal form
        if (canGotoEtheral && !_leftTriggerPressed && GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f)
        {
            switch (Form)
            {
                case Form.Normal:
                    Form = Form.Ethereal;
                    break;

                case Form.Void:
                    Form = Form.Ethereal;
                    break;

                case Form.Ethereal:
                    Form = Form.Normal;
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

    public void RotateAroundSingularity(Transform singularity)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        Vector2 input = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One);

        if (input == Vector2.zero)
            return;

        Vector3 dir = transform.position - singularity.position;
        float wantedAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        Quaternion targetRot = Quaternion.Euler(Vector3.forward * wantedAngle);
        singularity.rotation = Quaternion.RotateTowards(singularity.rotation, targetRot, 80 * Time.deltaTime);

        // DEBUGS
        Debug.DrawRay(singularity.position, dir);
        Debug.DrawRay(singularity.position, input);
    }
}
