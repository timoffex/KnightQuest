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
public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    bool followMouse = true;

    Character m_character;
    GameSingletons m_gameSingletons;

    Vector2 m_direction;
    bool m_alignedToMouse = false;

    public Vector2 Direction => m_direction;

    protected virtual void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
        m_character = GetComponentInParent<Character>();
    }

    protected virtual void Update()
    {
        m_alignedToMouse = false;

        if (followMouse)
        {
            AlignToMouse();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!m_alignedToMouse) AlignToMouse();
            Attack();
        }
    }

    protected abstract void Attack();

    void AlignToMouse()
    {
        AlignToAngleDegrees(
            Vector2.SignedAngle(
                Vector2.right,
                m_gameSingletons.MouseWorldPosition - (Vector2)transform.position));
        m_alignedToMouse = true;
    }

    void AlignToAngleDegrees(float degrees)
    {
        var rotation = Quaternion.Euler(0, 0, degrees);
        transform.rotation = rotation;

        m_direction = rotation * Vector2.right;
        transform.position = m_character.WeaponCenterPoint.position +
            m_character.WeaponRadius * (Vector3)m_direction;
    }
}
