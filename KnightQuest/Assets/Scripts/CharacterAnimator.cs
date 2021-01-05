using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private Animator m_animator;
    private Rigidbody2D m_rigidbody2D;
    private Character m_character;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_character = GetComponentInParent<Character>();
        m_rigidbody2D = GetComponentInParent<Rigidbody2D>();
    }

    void LateUpdate()
    {
        m_animator.SetFloat(
            "speedPercentage",
            Mathf.Clamp01(m_rigidbody2D.velocity.magnitude / m_character.maxSpeed));
        m_animator.SetInteger("direction", m_character.Direction.ToInteger());
    }
}
