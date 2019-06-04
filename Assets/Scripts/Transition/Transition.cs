using GamepadInput;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Transition
{
    #region Fields
    [Header("General Settings")]
    [SerializeField] private int _cinematicIndex = 0;
    [SerializeField] private Sprite _sprite = null;

    private int _currentDialogue = -1;
    private string _dialogueKey;
    private TextMeshProUGUI _text;
    #endregion

    public void LoadVignette(ref Image image, ref TextMeshProUGUI text)
    {
        _dialogueKey = "cinematics." + _cinematicIndex + ".";

        _text = text;

        image.sprite = _sprite;
        ChangeDialogue();
    }

    public void ChangeDialogue()
    {
        _currentDialogue++;

        string key = _dialogueKey + _currentDialogue;
        string text = Translation.Get(key);

        // if dialogue is ended
        if (text == key)
        {
            TransitionManager.Instance.ChangeTransition();
        }
        else
        {
            _text.text = text;
        }
    }
}
