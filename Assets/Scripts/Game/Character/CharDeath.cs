using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeathHandle(object sender);

public class CharDeath : MonoBehaviour
{
    public static readonly int DEATH_Y = -10;

    #region Fields
    public static event DeathHandle EventDeath;

    [SerializeField] private Material _materialNotActive;
    [SerializeField] private Material _materialActive;

    [HideInInspector] public Vector3 currentCheckpoint;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        currentCheckpoint = transform.position;
    }

    void Update()
    {
        if (transform.position.y <= DEATH_Y)
        {
            Death();
        }
    }

    void OnTriggerEnter(Collider other)
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
        CharFeedbacks.Instance.PlayDeathPS();
        transform.position = (Vector2)currentCheckpoint;

        AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Death);
        EventDeath?.Invoke(this);
    }
}
