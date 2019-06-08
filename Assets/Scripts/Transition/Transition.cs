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

    // cached var
    private TextMeshProUGUI _text;
    private Image _image;
    #endregion

    public void LoadVignette(Image image, TextMeshProUGUI text)
    {
        // variables attributions
        _text = text;
        _image = image;
        _image.sprite = _sprite;

        // dialogues
        _dialogueKey = "cinematics." + _cinematicIndex + ".";
        ChangeDialogue();

        // animation
        _image.CrossFadeAlpha(1, 1, false);
    }

    public void UnloadVignette()
    {
        _image.CrossFadeAlpha(0, 1, false);
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
