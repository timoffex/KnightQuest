using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SwordData))]
[RequireComponent(typeof(CombatOffense))]
public class Sword : Weapon
{
    SwordData m_swordAttackData;
    CombatOffense m_combatStatsModifier;
    Animator m_animator;
    AudioSource m_audioSource;

    float m_nextAttackTime;

    /// <summary>
    /// Set of attackables that the sword has hit in its current attack.
    /// 
    /// Used to avoid duplicate damage.
    /// </summary>
    readonly HashSet<Attackable> m_attackablesHitSoFar = new HashSet<Attackable>();

    public bool CoolingDown => Time.time < m_nextAttackTime;

    public virtual void OnCollidedWith(Collider2D other)
    {
        var attackable = other.GetComponent<Attackable>();
        if (attackable != null && !m_attackablesHitSoFar.Contains(attackable))
        {
            if (attackable.Hit(
                    Character.gameObject,
                    Direction * m_swordAttackData.attackStrength,
                    m_combatStatsModifier.Value))
                m_attackablesHitSoFar.Add(attackable);
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
                AttackIgnoreCooldown();
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

        m_swordAttackData = GetComponent<SwordData>();
        m_combatStatsModifier = GetComponent<CombatOffense>();
        m_animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
    }

    protected override void Attack()
    {
        if (!CoolingDown)
        {
            AttackIgnoreCooldown();
        }
    }

    void AttackIgnoreCooldown()
    {
        Debug.Log("Attacking!");
        m_audioSource.Play();
        m_animator.SetTrigger("attack");
        Character.AttackFreezeFrame(Direction);
        m_nextAttackTime = Time.time + m_swordAttackData.attackCooldown;
        m_attackablesHitSoFar.Clear();
        FreezeDirectionForAttack(m_swordAttackData.attackCooldown);
    }

    static Sword()
    {
        PersistableComponent.Register<Sword>("Sword");
    }
}
