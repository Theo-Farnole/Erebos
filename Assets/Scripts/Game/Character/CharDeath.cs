using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeathHandle(object sender);

public class CharDeath : MonoBehaviour
{
    #region Fields
    public static event DeathHandle EventDeath;

    [SerializeField] private Material _materialNotActive;
    [SerializeField] private Material _materialActive;

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
            currentCheckpoint = other.transform.position;
            other.GetComponent<Checkpoint>().ActiveBrasero();
        }
    }

    void OnDestroy()
    {
        EventDeath = null;
    }
    #endregion

    public void Death()
    {
        transform.position = (Vector2)currentCheckpoint;

        EventDeath?.Invoke(this);
    }
}
