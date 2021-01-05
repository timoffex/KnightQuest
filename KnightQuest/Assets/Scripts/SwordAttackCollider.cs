using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
