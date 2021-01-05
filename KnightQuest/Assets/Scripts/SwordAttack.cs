using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SwordAttack : MonoBehaviour
{
    Animator m_animator;
    GameSingletons m_gameSingletons;
    Quaternion m_initialRotation;

    public void OnCollidedWith(Collider2D other)
    {
        var attackable = other.GetComponent<Attackable>();
        if (attackable != null)
        {
            attackable.OnHit();
        }
    }

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;

        m_animator = GetComponent<Animator>();
        m_initialRotation = transform.rotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Rotate based on player direction.
            Vector2 clickPosition = m_gameSingletons.MouseWorldPosition;
            float angle =
                Vector2.SignedAngle(Vector2.right, clickPosition - (Vector2)transform.position);
            transform.rotation = m_initialRotation * Quaternion.Euler(0, 0, angle);

            // Trigger animation.
            m_animator.SetTrigger("attack");
        }
    }
}
