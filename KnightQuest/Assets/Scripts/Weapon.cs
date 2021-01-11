using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The base for every weapon's main script.
/// </summary>
/// <remarks>
/// A weapon is configured by attaching it to a <see cref="Character"/>. (Note to self: obviously
/// you can't reattach a weapon to a different character. Just instantiate a new one.)
/// </remarks>
[RequireComponent(typeof(WeaponData))]
public abstract class Weapon : MonoBehaviour
{
    WeaponData m_data;
    Character m_character;

    Vector2 m_direction;

    public Vector2 Direction => m_direction;

    public abstract void ControlAI(EnemyAI enemyAi);

    protected virtual void Awake()
    {
        m_data = GetComponent<WeaponData>();
        m_character = GetComponentInParent<Character>();
    }

    protected virtual void Start()
    {
    }

    protected virtual void OnEnable()
    {
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
        m_character.CurrentWeapon = null;
    }

    protected virtual void Update()
    {
    }

    public virtual void OnAttackButtonDown()
    {
        Attack();
    }

    public virtual void OnAttackButtonUp() { }

    public void AlignToward(Vector2 position)
    {
        AlignToAngleDegrees(
            Vector2.SignedAngle(
                Vector2.right,
                position - (Vector2)transform.position));
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
