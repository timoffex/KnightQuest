using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BowAttackData))]
[RequireComponent(typeof(CombatStatsModifier))]
public class BowAttack : Weapon
{
    BowAttackData m_bowAttackData;
    CombatStatsModifier m_statsModifier;

    float m_chargeStartTime = 0;

    bool m_charging = false;

    public float MaximumRange => m_bowAttackData.arrowSpeed * m_bowAttackData.arrowLiveTime;

    public float ChargePercentage =>
        m_charging
            ? Mathf.Clamp01((Time.time - m_chargeStartTime) / m_bowAttackData.chargeTime)
            : 0;

    public override void ControlAI(EnemyAI enemyAi)
    {
        if (enemyAi.DistanceToTarget < 0.8f * MaximumRange)
        {
            AlignToward(enemyAi.TargetPosition);
            if (!m_charging)
            {
                BeginCharging();
            }

            if (ChargePercentage > 0.9f)
            {
                ReleaseArrow();
            }
        }
        else
        {
            m_charging = false;
        }

        if (enemyAi.DistanceToTarget > 0.5f * MaximumRange)
        {
            enemyAi.MoveTowardTarget();
        }
    }

    public override void OnAttackButtonDown()
    {
        BeginCharging();
    }

    public override void OnAttackButtonUp()
    {
        ReleaseArrow();
    }

    protected override void Awake()
    {
        base.Awake();
        m_bowAttackData = GetComponent<BowAttackData>();
        m_statsModifier = GetComponent<CombatStatsModifier>();
    }

    protected override void Attack()
    {
        m_bowAttackData.arrowSpawner.Spawn(
            Character.gameObject,
            transform.position,
            Direction * m_bowAttackData.arrowSpeed * ChargePercentage,
            m_bowAttackData.arrowLiveTime,
            m_statsModifier);
    }

    void BeginCharging()
    {
        m_chargeStartTime = Time.time;
        m_charging = true;
    }

    void ReleaseArrow()
    {
        Attack();
        m_charging = false;
    }
}
