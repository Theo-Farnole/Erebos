using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Erebos.Inputs
{
    public static class InputProxy
    {
        public static bool SkipTutorial
        {
            get
            {
                return InputProxy.Menu.Validate || Input.GetKeyDown(KeyCode.Space);

            }
        }

        public class Menu
        {
            public static bool Back
            {
                get
                {
                    return Input.GetKeyDown(KeyCode.Escape) || GamePad.GetButtonDown(GamePad.Button.B, GamePad.Index.One);
                }
            }

            public static bool Validate
            {
                get
                {
                    return Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Space) || GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One);
                }
            }
        }

        public class Character
        {
            private static bool _rightTriggerDown = false;
            private static bool _leftTriggerDown = false;

            public static float Horizontal
            {
                get
                {
                    float horizontal = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).x;

                    if (horizontal == 0)
                    {
                        if (Input.GetKey(KeyCode.A))
                        {
                            horizontal--;
                        }
                        if (Input.GetKey(KeyCode.D))
                        {
                            horizontal++;
                        }
                    }

                    return horizontal;
                }
            }

            public static float Vertical
            {
                get
                {
                    float vertical = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.One).y;

                    if (vertical == 0)
                    {
                        if (Input.GetKey(KeyCode.W))
                        {
                            vertical++;
                        }
                        if (Input.GetKey(KeyCode.S))
                        {
                            vertical--;
                        }
                    }

                    return vertical;
                }
            }

            public static bool JumpDown
            {
                get
                {
                    bool jump = GamePad.GetButtonDown(GamePad.Button.A, GamePad.Index.One) ||
                                Input.GetKeyDown(KeyCode.Space);

                    return jump;
                }
            }

            public static bool JumpUp
            {
                get
                {
                    bool jump = GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.One) ||
                                Input.GetKeyUp(KeyCode.Space);

                    return jump;
                }
            }

            public static bool Unstick
            {
                get
                {
                    bool unstick = GamePad.GetButton(GamePad.Button.B, GamePad.Index.Any) ||
                        Input.GetKey(KeyCode.B);

                    return unstick;
                }
            }

            public static Vector2 LeftInput
            {
                get
                {
                    Vector2 input = GamePad.GetAxis(GamePad.Axis.LeftStick, GamePad.Index.Any);

                    if (input == Vector2.zero)
                    {
                        input.x = Horizontal;
                        input.y = Vertical;
                    }

                    return input;
                }
            }

            public static bool SwitchForm
            {
                get
                {
                    bool rightTrigger = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.One) > 0f;
                    bool leftTrigger = GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, GamePad.Index.One) > 0f;

                    // TriggerDown
                    if ((rightTrigger && !_rightTriggerDown) || (leftTrigger && !_leftTriggerDown))
                    {
                        _rightTriggerDown = rightTrigger;
                        _leftTriggerDown = leftTrigger;

                        return true;
                    }

                    _rightTriggerDown = rightTrigger;
                    _leftTriggerDown = leftTrigger;

                    return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
                }
            }
        }
    }
}
