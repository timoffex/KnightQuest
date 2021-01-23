using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BowAnimator : MonoBehaviour
{
    Animator m_animator;
    Bow m_bowAttack;

    protected virtual void Start()
    {
        m_animator = GetComponent<Animator>();
        m_bowAttack = GetComponentInParent<Bow>();
    }

    protected virtual void Update()
    {
        m_animator.SetFloat("drawPercentage", m_bowAttack.ChargePercentage);
    }
}
