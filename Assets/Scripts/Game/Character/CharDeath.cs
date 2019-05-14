using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDeath : MonoBehaviour
{
    #region Fields
    [HideInInspector] public Vector3 currentCheckpoint;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        currentCheckpoint = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KillingObstacle"))
        {
            Death();
        }

        if (other.CompareTag("Checkpoint"))
        {
            SetNewCheckpoint(other);
        }
    }
    #endregion

    private void Death()
    {
        Debug.Log("Death");

        CharControllerManager.Instance.Attracted = false;
        CharController.Instance.ResetMovements();

        transform.position = (Vector2)currentCheckpoint;
    }

    private void SetNewCheckpoint(Collider other)
    {
        Debug.Log("New checkpoint is " + other.name);
        currentCheckpoint = other.transform.position;
    }
}
