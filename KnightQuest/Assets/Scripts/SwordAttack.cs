using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SwordAttackData))]
[RequireComponent(typeof(CombatStatsModifier))]
public class SwordAttack : Weapon
{
    SwordAttackData m_data;
    CombatStatsModifier m_combatStatsModifier;
    Animator m_animator;
    Character m_character;

    float m_nextAttackTime;

    public bool CoolingDown => Time.time < m_nextAttackTime;

    public virtual void OnCollidedWith(Collider2D other)
    {
        var attackable = other.GetComponent<Attackable>();
        if (attackable != null)
        {
            attackable.OnHit(Direction * m_data.attackStrength, m_combatStatsModifier);
        }
    }

    public override void ControlAI(EnemyAI enemyAi)
    {
        if (enemyAi.DistanceToTarget < m_character.WeaponRadius + m_data.extraAttackRange)
        {
            if (!CoolingDown)
            {
                AlignToward(enemyAi.TargetPosition);
                AttackNoCooldown();
            }
        }
        else
        {
            enemyAi.MoveTowardTarget();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        m_data = GetComponent<SwordAttackData>();
        m_combatStatsModifier = GetComponent<CombatStatsModifier>();
        m_animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();

        m_character = GetComponentInParent<Character>();
    }

    protected override void Attack()
    {
        if (!CoolingDown)
        {
            AttackNoCooldown();
        }
    }

    void AttackNoCooldown()
    {
        Debug.Log("Attacking!");
        m_animator.SetTrigger("attack");
        m_character.AttackFreezeFrame(Direction);
        m_nextAttackTime = Time.time + m_data.attackCooldown;
    }
}
