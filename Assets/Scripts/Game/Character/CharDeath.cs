using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharDeath : MonoBehaviour
{
    #region Fields
    [SerializeField] private Material _materialNotActive;
    [SerializeField] private Material _materialActive;

    [HideInInspector] public Transform currentCheckpoint;

    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        currentCheckpoint = transform;
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

        transform.position = (Vector2)currentCheckpoint.position;
    }

    private void SetNewCheckpoint(Collider other)
    {
        // set not active material to the old checkpoint 
        if (currentCheckpoint != transform)
        {
            var oldMat = currentCheckpoint.GetChild(0).GetComponentsInChildren<MeshRenderer>();

            foreach (var m in oldMat)
            {
                m.material = _materialNotActive;
            }
        }


        // change current checkpoint
        currentCheckpoint = other.transform;

        // then apply new material
        MeshRenderer[] newMat = currentCheckpoint.GetChild(0).GetComponentsInChildren<MeshRenderer>();

        foreach (var m in newMat)
        {
            m.material = _materialActive;
        }
    }
}
