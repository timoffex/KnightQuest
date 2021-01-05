﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [Tooltip("Maximum speed in units / second.")]
    public float maxSpeed;

    public CharacterDirection Direction { get; private set; }

    Rigidbody2D m_rigidbody2D;
    GameSingletons m_gameSingletons;
    float m_freezeDirectionUntilTime;

    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetLookDirection(m_gameSingletons.MouseWorldPosition - (Vector2)transform.position);
            m_freezeDirectionUntilTime = Time.time + 0.3f;
        } else if (Time.time > m_freezeDirectionUntilTime)
        {
            SetLookDirection(m_rigidbody2D.velocity);
        }
    }

    void SetLookDirection(Vector2 dir) {
        if (Mathf.Approximately(dir.sqrMagnitude, 0))
            return;

        Direction = CharacterDirectionUtils.CharacterDirectionFromNonzero(dir);
    }
}