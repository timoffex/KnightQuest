using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BowAnimator : MonoBehaviour
{
    Animator m_animator;
    BowAttack m_bowAttack;

    protected virtual void Start()
    {
        m_animator = GetComponent<Animator>();
        m_bowAttack = GetComponentInParent<BowAttack>();
    }

    protected virtual void Update()
    {
        m_animator.SetFloat("drawPercentage", m_bowAttack.ChargePercentage);
    }
}
