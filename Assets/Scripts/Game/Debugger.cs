using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour
{
    void Update()
    {
        if (GamePad.GetButtonUp(GamePad.Button.Start, GamePad.Index.One))
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
