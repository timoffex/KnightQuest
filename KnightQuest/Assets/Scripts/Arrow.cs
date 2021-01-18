using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controller script for an arrow that can hit <see cref="Attackable"/> instances.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ArrowData))]
public class Arrow : PersistableComponent
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

    public void Hit(Attackable attackable)
    {
        if (attackable.Hit(
                m_attacker,
                m_rigidbody2D.velocity * m_rigidbody2D.mass * m_data.hitImpulseMultiplier,
                m_statsModifier))
        {
            StopBeingDangerous();
        }
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        // Time to live
        writer.WriteFloat(m_deathTime - Time.time);

        // TODO: Save/load attacker and stats modifier
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        // Time to live
        m_deathTime = Time.time + reader.ReadFloat();
    }

    protected override void Awake()
    {
        base.Awake();
        m_data = GetComponent<ArrowData>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (Time.time > m_deathTime)
            StopBeingDangerous();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        StopBeingDangerous();
    }
    protected virtual void StopBeingDangerous()
    {
        Destroy(gameObject);
        m_data.remainsSpawner?.Spawn(
            position: transform.position,
            rotation: transform.rotation,
            velocity: m_rigidbody2D.velocity,
            angularVelocity: m_rigidbody2D.angularVelocity);
    }
}
