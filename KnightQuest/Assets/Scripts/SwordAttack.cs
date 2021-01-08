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

    public virtual void OnCollidedWith(Collider2D other)
    {
        var attackable = other.GetComponent<Attackable>();
        if (attackable != null)
        {
            attackable.OnHit(Direction * m_data.attackStrength, m_combatStatsModifier);
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
        m_animator.SetTrigger("attack");
        m_character.AttackFreezeFrame(Direction);
    }
}
