using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SwordAttack : Weapon
{
    [SerializeField] float attackStrength = 1;

    Animator m_animator;
    Character m_character;

    public void OnCollidedWith(Collider2D other)
    {
        var attackable = other.GetComponent<Attackable>();
        if (attackable != null)
        {
            attackable.OnHit(Direction * attackStrength);
        }
    }

    protected override void Start()
    {
        base.Start();

        m_character = GetComponentInParent<Character>();
        m_animator = GetComponent<Animator>();
    }

    protected override void Attack()
    {
        m_animator.SetTrigger("attack");
        m_character.AttackFreezeFrame(Direction);
    }
}
