using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputProxy
{
    public static bool SkipTutorial
    {
        get
        {
            return GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One) || GamePad.GetButtonDown(GamePad.Button.Start, GamePad.Index.One);
        }
    }
}
