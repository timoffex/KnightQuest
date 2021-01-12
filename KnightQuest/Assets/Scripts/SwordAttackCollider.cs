using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Invokes <see cref="SwordAttack.OnCollidedWith(UnityEngine.Collider2D)"/>  on a parent object
/// when anything enters the trigger attached to this object.
[RequireComponent(typeof(Collider2D))]
public class SwordAttackCollider : MonoBehaviour
{
    SwordAttack m_swordAttack;

    void Start()
    {
        m_swordAttack = GetComponentInParent<SwordAttack>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        m_swordAttack.OnCollidedWith(collider);
    }
}
