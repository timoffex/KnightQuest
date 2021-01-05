using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SwordAttack : MonoBehaviour
{
    private Animator m_animator;
    private GameSingletons m_gameSingletons;
    private Quaternion m_initialRotation;

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
            Vector2 clickPosition = m_gameSingletons.MouseWorldPosition;
            
            // Rotate based on player direction.
            float angle = 
                Vector2.SignedAngle(Vector2.right, clickPosition - (Vector2)transform.position);

            transform.rotation = m_initialRotation * Quaternion.Euler(0, 0, angle);

            m_animator.SetTrigger("attack");
        }
    }
}
