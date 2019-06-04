using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeathHandle(object sender);
public delegate void RespawnHandle(object sender);

public class CharDeath : MonoBehaviour
{
    public static readonly int DEATH_Y = -10;
    public static readonly float RESPAWN_TIME = 0.8f;

    #region Fields
    public static event DeathHandle EventDeath;
    public static event RespawnHandle EventRespawn;

    public static bool isDead = false;

    [HideInInspector] public Vector3 currentCheckpoint;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        currentCheckpoint = transform.position;

        // call respawn event with delay
        DeathHandle d1 = new DeathHandle(InvokeRespawn);
        EventDeath += d1;
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
        EventRespawn = null;
    }
    #endregion

    public void Death()
    {
        // avoid 2 death straight
        if (isDead)
            return;

        isDead = true;
        AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Death);

        EventDeath?.Invoke(this);
    }

    private void InvokeRespawn(object sender)
    {
        StartCoroutine(CustomDelay.ExecuteAfterTime(RESPAWN_TIME, () =>
        {
            transform.position = (Vector2)currentCheckpoint;

            isDead = false;
            EventRespawn?.Invoke(this);
        }));
    }
}
