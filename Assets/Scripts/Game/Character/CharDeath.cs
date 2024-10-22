﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeathHandle();
public delegate void RespawnHandle();

public class CharDeath : MonoBehaviour
{
    public static readonly int DEATH_Y = -10;
    public static readonly float RESPAWN_TIME = 0.8f;

    #region Fields
    public static event DeathHandle EventDeath;
    public static event RespawnHandle EventRespawn;

    public static bool isDead = false;

    private Vector3 _currentCheckpoint;
    private CharController _charController = null;
    #endregion

    #region MonoBehaviour Callbacks
    void Awake()
    {
        isDead = false;
        _currentCheckpoint = transform.position;

        _charController = GetComponent<CharController>();

        // call respawn event with delay
        DeathHandle d1 = new DeathHandle(InvokeRespawn);
        EventDeath += d1;
    }

    void Update()
    {
        if (transform.position.y <= DEATH_Y || _charController.IsBlocked)
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
            if (_currentCheckpoint != other.transform.position)
            {
                GameManager.Instance.ValidateCollectibles();
            }

            _currentCheckpoint = other.transform.position;
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

        EventDeath?.Invoke();
    }

    private void InvokeRespawn()
    {
        this.ExecuteAfterTime(RESPAWN_TIME, () =>
        {
            transform.position = (Vector2)_currentCheckpoint;

            isDead = false;
            EventRespawn?.Invoke();
            AudioManager.Instance.PlaySoundGeneral(SoundGeneral.Respawn);
        });
    }
}
