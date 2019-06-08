using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeactivateOnScene : MonoBehaviour
{
    [SerializeField] private List<string> _deactivateOnScenes = new List<string>();

    void Awake()
    {
        for (int i = 0; i < _deactivateOnScenes.Count; i++)
        {
            if (SceneManager.GetActiveScene().name == _deactivateOnScenes[i])
            {
                gameObject.SetActive(false);
            }
        }
    }
}
