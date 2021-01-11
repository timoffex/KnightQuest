using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controller script for an arrow that can hit <see cref="Attackable"/> instances.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ArrowData))]
public class Arrow : MonoBehaviour
{
    ArrowData m_data;
    Rigidbody2D m_rigidbody2D;
    Collider2D m_collider2D;

    GameObject m_attacker;
    float m_deathTime;
    CombatStatsModifier m_statsModifier;

    public virtual void Initialize(
        GameObject attacker,
        Vector2 velocity,
        float liveTime,
        CombatStatsModifier statsModifier)
    {
        m_attacker = attacker;
        m_statsModifier = statsModifier;

        transform.rotation = Quaternion.FromToRotation(Vector2.right, velocity);
        m_rigidbody2D.velocity = velocity;

        m_deathTime = Time.time + liveTime;
    }

    protected virtual void Awake()
    {
        m_data = GetComponent<ArrowData>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (Time.time > m_deathTime)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        var attackable = collider.GetComponent<Attackable>();
        if (attackable != null &&
            attackable.Hit(
                m_attacker,
                m_rigidbody2D.velocity * m_rigidbody2D.mass,
                m_statsModifier))
        {
            Destroy(gameObject);
        }
    }
}
