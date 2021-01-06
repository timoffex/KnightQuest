using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BowAttackData))]
public class BowAttack : Weapon
{
    BowAttackData m_data;

    protected override void Awake()
    {
        base.Awake();
        m_data = GetComponent<BowAttackData>();
    }

    protected override void Attack()
    {
        m_data.arrowSpawner.Spawn(transform.position, Direction * m_data.arrowSpeed);
    }
}
