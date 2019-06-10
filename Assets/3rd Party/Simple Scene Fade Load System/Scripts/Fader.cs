using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    #region Fields
    [HideInInspector] public bool start = false;
    [HideInInspector] public float fadeDamp = 0.0f;
    [HideInInspector] public AsyncOperation ao;
    [HideInInspector] public float alpha = 0.0f;
    [HideInInspector] public Color fadeColor;
    [HideInInspector] public bool isFadeIn = false;

    private CanvasGroup _myCanvas;
    private Image _background;
    private float _lastTime = 0;
    private bool _startedLoading = false;
    #endregion

    #region MonoBehaviour Callbacks
    void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnEnable()
    {
        //Set callback
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Remove callback
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    #endregion

    public void InitiateFader()
    {
        DontDestroyOnLoad(gameObject);

        _myCanvas = transform.GetComponent<CanvasGroup>();
        _background = transform.GetComponent<Image>();

        if (_myCanvas && _background)
        {
            _background.color = fadeColor;
            _myCanvas.alpha = 0.0f;

            StartCoroutine(FadeIt());
        }
        else
        {
            Debug.LogWarning("Something is missing please reimport the package.");
        }
    }

    IEnumerator FadeIt()
    {
        _lastTime = Time.time;

        float coDelta = _lastTime;
        bool hasFadedIn = false;

        while (!hasFadedIn)
        {
            coDelta = Time.time - _lastTime;

            if (!isFadeIn)
            {
                //Fade in
                alpha = GetNewAlpha(coDelta, 1, alpha);
                if (alpha == 1 && !_startedLoading)
                {
                    _startedLoading = true;
                    ao.allowSceneActivation = true;
                }
            }
            else
            {
                //Fade out
                alpha = GetNewAlpha(coDelta, 0, alpha);
                if (alpha == 0)
                {
                    hasFadedIn = true;
                }
            }

            _lastTime = Time.time;
            _myCanvas.alpha = alpha;
            yield return null;
        }


        Debug.Log("Your scene has been loaded , and fading in has just ended");

        Destroy(gameObject);

        yield return null;
    }


    float GetNewAlpha(float delta, int to, float currAlpha)
    {
        switch (to)
        {
            case 0:
                currAlpha -= fadeDamp * delta;


                break;
            case 1:
                currAlpha += fadeDamp * delta;

                break;
        }

        currAlpha = Mathf.Clamp(currAlpha, 0, 1);

        return currAlpha;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIt());
        
        isFadeIn = true;
    }
}
