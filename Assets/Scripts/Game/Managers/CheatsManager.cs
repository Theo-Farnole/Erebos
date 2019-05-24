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

    void Update()
    {
        if (GamePad.GetButtonDown(GamePad.Button.Back, GamePad.Index.One))
        {
            _isActive = !_isActive;

            if (_isActive)
            {
                if (_checkpoints == null)
                {
                    _checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
                }

                CharControllerSingularity charControllerSingularity = CharControllerManager.Instance.GetComponent<CharControllerSingularity>();
                charControllerSingularity.canGotoEtheral = true;
                charControllerSingularity.canGotoVoid = true;
            }
        }

        if (_isActive)
        {
            int targetCP = -1;

            if (Input.GetKeyDown(KeyCode.Keypad1)) targetCP = 0;
            else if (Input.GetKeyDown(KeyCode.Keypad2)) targetCP = 1;
            else if (Input.GetKeyDown(KeyCode.Keypad3)) targetCP = 2;
            else if (Input.GetKeyDown(KeyCode.Keypad4)) targetCP = 3;
            else if (Input.GetKeyDown(KeyCode.Keypad5)) targetCP = 4;
            else if (Input.GetKeyDown(KeyCode.Keypad6)) targetCP = 5;
            else if (Input.GetKeyDown(KeyCode.Keypad7)) targetCP = 6;
            else if (Input.GetKeyDown(KeyCode.Keypad8)) targetCP = 7;
            else if (Input.GetKeyDown(KeyCode.Keypad9)) targetCP = 8;

            Debug.Log("targetCp: " + targetCP);

            if (targetCP != -1 && targetCP < _checkpoints.Length)
            {
                CharControllerManager.Instance.transform.position = _checkpoints[targetCP].transform.position;
                Debug.Log("TP to : " + _checkpoints[targetCP].transform + " > " + _checkpoints[targetCP].transform.position);
            }
        }
    }

    void OnGUI()
    {
        if (!_isActive)
            return;

        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(w - 60, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = Color.red;
        string text = string.Format("cheats enabled");
        GUI.Label(rect, text, style);
    }
}
