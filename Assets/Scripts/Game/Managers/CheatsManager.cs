using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatsManager : MonoBehaviour
{
    #region Fields
    [SerializeField] private bool _isActive = false;

    private GameObject[] _checkpoints;
    #endregion

    #region Properties
    public GameObject[] Checkpoints
    {
        get
        {
            if (_checkpoints == null)
            {
                _checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            }
            return _checkpoints;
        }
    }
    #endregion

    void Update()
    {
        if (GamePad.GetButtonDown(GamePad.Button.Back, GamePad.Index.One) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            _isActive = !_isActive;

            if (_isActive)
            {
                CharControllerSingularity charControllerSingularity = CharControllerManager.Instance.GetComponent<CharControllerSingularity>();

                charControllerSingularity.canGotoEtheral = true;
                charControllerSingularity.canGotoVoid = true;
            }
        }

        if (_isActive)
        {
            int targetCP = -1;

            if (Input.GetKeyDown(KeyCode.Keypad0)) EndLevel();
            else if (Input.GetKeyDown(KeyCode.Keypad1)) targetCP = 0;
            else if (Input.GetKeyDown(KeyCode.Keypad2)) targetCP = 1;
            else if (Input.GetKeyDown(KeyCode.Keypad3)) targetCP = 2;
            else if (Input.GetKeyDown(KeyCode.Keypad4)) targetCP = 3;
            else if (Input.GetKeyDown(KeyCode.Keypad5)) targetCP = 4;
            else if (Input.GetKeyDown(KeyCode.Keypad6)) targetCP = 5;
            else if (Input.GetKeyDown(KeyCode.Keypad7)) targetCP = 6;
            else if (Input.GetKeyDown(KeyCode.Keypad8)) targetCP = 7;
            else if (Input.GetKeyDown(KeyCode.Keypad9)) targetCP = 8;

            if (targetCP != -1 && targetCP < Checkpoints.Length)
            {
                CharControllerManager.Instance.transform.position = Checkpoints[targetCP].transform.position;
            }
        }
    }

    void EndLevel()
    {
        Transform bigCheckpoint = FindObjectOfType<TriggerToTransition>().transform;

        CharControllerManager.Instance.transform.position = bigCheckpoint.position;
    }

    void OnGUI()
    {
        if (!_isActive)
            return;

        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect topRight = new Rect(w - 60, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.red;
        GUI.Label(topRight, "cheats enabled", style);

        float t = GameState.CurrentSpeedrunTime;

        int min = Mathf.FloorToInt(t / 60);
        int sec = Mathf.FloorToInt(t % 60);
        string txt = string.Format(min.ToString("00") + ":" + sec.ToString("00"));

        Rect topLeft = new Rect(0, 0, w, h * 2 / 100);
        GUI.Label(topLeft, txt, style);
    }
}
