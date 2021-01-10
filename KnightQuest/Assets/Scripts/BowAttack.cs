﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BowAttackData))]
[RequireComponent(typeof(CombatStatsModifier))]
public class BowAttack : Weapon
{
    BowAttackData m_data;
    CombatStatsModifier m_statsModifier;

    float m_chargeStartTime = 0;

    bool m_charging = false;

    protected float ChargePercentage =>
        m_charging ? (Time.time - m_chargeStartTime) / m_data.chargeTime : 0;

    protected override void Awake()
    {
        base.Awake();
        m_data = GetComponent<BowAttackData>();
        m_statsModifier = GetComponent<CombatStatsModifier>();
    }

    protected override void OnAttackButtonDown()
    {
        m_chargeStartTime = Time.time;
        m_charging = true;
    }

    protected override void OnAttackButtonUp()
    {
        try
        {
            Attack();
        }
        finally
        {
            m_charging = false;
        }
    }

    protected override void Attack()
    {
        m_data.arrowSpawner.Spawn(
            transform.position,
            Direction * m_data.arrowSpeed * ChargePercentage,
            m_statsModifier);
    }
}
