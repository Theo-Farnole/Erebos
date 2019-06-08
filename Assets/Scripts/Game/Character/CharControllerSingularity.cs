using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Form { Normal, Ethereal, Void };
public delegate void FormHandle(Form form);

public class CharControllerSingularity : MonoBehaviour
{
    #region Fields
    #region Publics Fields
    public static event FormHandle EventForm;

    [HideInInspector] public bool canGotoVoid = true;
    [HideInInspector] public bool canGotoEtheral = true;
    #endregion

    #region Serialize Fields
    [SerializeField] private float _anglesPerSecond = 90;
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

            switch (_form)
            {
                case Form.Ethereal:
                    AudioManager.Instance.PlaySoundGeneral(SoundGeneral.FormToWhite);
                    break;

                case Form.Void:
                    AudioManager.Instance.PlaySoundGeneral(SoundGeneral.FormToBlack);
                    break;
            }

            EventForm?.Invoke(_form);
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

        // on death, return to normal form
        DeathHandle d = new DeathHandle(() =>
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
        if (CharDeath.isDead)
            return;

        if (!_rightTriggerPressed && GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One) > 0f ||
         !_leftTriggerPressed && GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f)
        {
            switch (Form)
            {
                case Form.Normal:
                case Form.Ethereal:
                    if (canGotoVoid)
                    {
                        Form = Form.Void;
                    }
                    break;

                case Form.Void:
                    if (canGotoEtheral)
                    {
                        Form = Form.Ethereal;
                    }
                    else
                    {
                        Form = Form.Normal;
                    }
                    break;
            }
        }

        _rightTriggerPressed = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One) > 0f ? true : false;
        _leftTriggerPressed = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f ? true : false;
    }

    public void RotateAroundSingularity(Transform singularity, float currentAngleDelta)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        Vector2 input = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One);

        if (input != Vector2.zero)
        {
            float wantedAngle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

            Vector3 oldSingularityEulerAngles = singularity.eulerAngles;
            Quaternion targetRot = Quaternion.Euler(Vector3.forward * (wantedAngle - currentAngleDelta));
            singularity.rotation = Quaternion.RotateTowards(singularity.rotation, targetRot, _anglesPerSecond * Time.deltaTime);
        }

        // mask
        Transform blackMask = CharFeedbacks.Instance.BlackMask.transform;
        Vector3 dir = (singularity.position - blackMask.position).normalized;

        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        blackMask.localEulerAngles = angle * Vector3.forward;
    }
}
