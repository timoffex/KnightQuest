using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SwordAttack : MonoBehaviour
{
    [SerializeField] float attackStrength = 1;

    Animator m_animator;
    GameSingletons m_gameSingletons;
    Quaternion m_initialRotation;
    Character m_character;

    Vector2 m_attackDirection;

    public void OnCollidedWith(Collider2D other)
    {
        var attackable = other.GetComponent<Attackable>();
        if (attackable != null)
        {
            attackable.OnHit(m_attackDirection * attackStrength);
        }
    }

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;

        m_character = GetComponentInParent<Character>();
        m_animator = GetComponent<Animator>();
        m_initialRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Rotate based on player direction.
            var clickPosition = m_gameSingletons.MouseWorldPosition;
            m_attackDirection = clickPosition - (Vector2)transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, m_attackDirection);
            transform.rotation = m_initialRotation * Quaternion.Euler(0, 0, angle);

            // Trigger animation, which should enable colliders.
            m_animator.SetTrigger("attack");
            m_character.AttackFreezeFrame(clickPosition - (Vector2)m_character.transform.position);
        }
    }
}
