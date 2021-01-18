using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SwordAttackData))]
[RequireComponent(typeof(CombatStatsModifier))]
public class SwordAttack : Weapon
{
    SwordAttackData m_swordAttackData;
    CombatStatsModifier m_combatStatsModifier;
    Animator m_animator;

    float m_nextAttackTime;

    public bool CoolingDown => Time.time < m_nextAttackTime;

    public virtual void OnCollidedWith(Collider2D other)
    {
        var attackable = other.GetComponent<Attackable>();
        if (attackable != null)
        {
            attackable.Hit(
                Character.gameObject,
                Direction * m_swordAttackData.attackStrength,
                m_combatStatsModifier.Value);
        }
    }

    public override void ControlAI(EnemyAI enemyAi)
    {
        if (enemyAi.DistanceToTarget <
                Character.WeaponRadius + m_swordAttackData.extraAttackRange)
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

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        // Cooldown
        writer.WriteFloat(m_nextAttackTime - Time.time);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        // Cooldown
        m_nextAttackTime = Time.time + reader.ReadFloat();
    }

    protected override void Awake()
    {
        base.Awake();

        m_swordAttackData = GetComponent<SwordAttackData>();
        m_combatStatsModifier = GetComponent<CombatStatsModifier>();
        m_animator = GetComponent<Animator>();
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
        Character.AttackFreezeFrame(Direction);
        m_nextAttackTime = Time.time + m_swordAttackData.attackCooldown;
    }
}
