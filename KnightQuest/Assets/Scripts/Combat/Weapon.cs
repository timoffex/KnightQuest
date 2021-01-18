using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base for every weapon's main script.
/// </summary>
/// <remarks>
/// A weapon must be instantiated with a <see cref="Character"/> parent. (Note to self: obviously
/// you can't reattach a weapon to a different character. Just instantiate a new one.)
/// 
/// Weapons can provide a <see cref="CombatDefense"/>.
/// </remarks>
[RequireComponent(typeof(WeaponData))]
public abstract class Weapon : PersistableComponent
{
    WeaponData m_data;
    Character m_character;

    Vector2 m_direction;
    float m_unfreezeDirectionTime = float.NegativeInfinity;

    public Vector2 Direction => m_direction;

    /// <summary>
    /// The effect of this weapon on a character's defenses. May be null.
    /// </summary>
    public CombatDefense CombatDefense { get; private set; }

    public abstract void ControlAI(EnemyAI enemyAi);

    protected Character Character => m_character;

    protected override void Awake()
    {
        base.Awake();
        m_data = GetComponent<WeaponData>();
    }

    protected virtual void Start()
    {
        CombatDefense = GetComponent<CombatDefense>();
        m_character = GetComponentInParent<Character>();

        if (m_character.CurrentWeapon != null)
        {
            Debug.LogError(
                $"Multiple enabled weapons on {m_character} (current: {m_character.CurrentWeapon})",
                this);
        }
        else
        {
            m_character.CurrentWeapon = this;
        }
    }

    protected virtual void OnDisable()
    {
        // Character may be null if it is being destroyed.
        if (m_character != null)
            m_character.CurrentWeapon = null;
    }

    public virtual void OnAttackButtonDown()
    {
        Attack();
    }

    public virtual void OnAttackButtonUp() { }

    public void AlignToward(Vector2 position)
    {
        if (Time.time > m_unfreezeDirectionTime)
        {
            AlignToAngleDegrees(
                Vector2.SignedAngle(
                    Vector2.right,
                    position - (Vector2)transform.position));
        }
    }

    protected void FreezeDirectionForAttack(float duration)
    {
        m_unfreezeDirectionTime = Time.time + duration;
    }

    protected abstract void Attack();

    void AlignToAngleDegrees(float degrees)
    {
        var rotation = Quaternion.Euler(0, 0, degrees);
        transform.rotation = rotation;

        m_direction = rotation * Vector2.right;
        transform.position = m_character.WeaponCenterPoint.position +
            m_character.WeaponRadius * (Vector3)m_direction;
    }
}
