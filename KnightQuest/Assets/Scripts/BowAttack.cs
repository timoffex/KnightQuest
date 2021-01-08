using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BowAttackData))]
[RequireComponent(typeof(CombatStatsModifier))]
public class BowAttack : Weapon
{
    BowAttackData m_data;
    CombatStatsModifier m_statsModifier;

    protected override void Awake()
    {
        base.Awake();
        m_data = GetComponent<BowAttackData>();
        m_statsModifier = GetComponent<CombatStatsModifier>();
    }

    protected override void Attack()
    {
        m_data.arrowSpawner.Spawn(
            transform.position, Direction * m_data.arrowSpeed, m_statsModifier);
    }
}
