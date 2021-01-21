using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controller script for an arrow that can hit <see cref="Attackable"/> instances.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(ArrowData))]
public class Arrow : PersistableComponent, IIgnitable
{
    ArrowData m_data;
    Rigidbody2D m_rigidbody2D;
    Collider2D m_collider2D;

    GameObject m_attacker;
    float m_deathTime;
    float m_initialSpeed;
    ArrowCombatOffense.Modification m_statsModification;
    bool m_isNeutralized = false;

    bool m_isIgnited = false;

    /// <summary>
    /// The child fire controller. This should only be set by the
    /// <see cref="ArrowFireAttachment"/> class.
    /// </summary>
    public ArrowFireAttachment FireAttachment { private get; set; }

    public virtual void Initialize(
        GameObject attacker,
        Vector2 velocity,
        float liveTime,
        ArrowCombatOffense.Modification statsModification)
    {
        m_attacker = attacker;
        m_statsModification = statsModification;

        transform.rotation = Quaternion.FromToRotation(Vector2.right, velocity);
        m_rigidbody2D.velocity = velocity;
        m_initialSpeed = velocity.magnitude;

        m_deathTime = Time.time + liveTime;
    }

    public void Hit(Attackable attackable)
    {
        var modificationWithSpeed = m_statsModification
            .WithSpeedMultiplier(Mathf.Clamp01(m_rigidbody2D.velocity.magnitude / m_initialSpeed))
            .WithFireDamage(m_isIgnited ? m_data.fireDamage : 0);

        if (attackable.Hit(
                m_attacker,
                m_rigidbody2D.velocity * m_rigidbody2D.mass * m_data.hitImpulseMultiplier,
                modificationWithSpeed))
        {
            StopBeingDangerous(randomAngularVelocity: true);
        }
    }

    public void Ignite()
    {
        FireAttachment.Ignite();
        m_isIgnited = true;
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);

        // Time to live
        writer.WriteFloat(m_deathTime - Time.time);

        writer.WriteFloat(m_initialSpeed);
        m_statsModification.Save(writer);

        writer.WriteBool(m_isIgnited);

        // TODO: Save/load attacker
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);

        // Time to live
        m_deathTime = Time.time + reader.ReadFloat();

        m_initialSpeed = reader.ReadFloat();
        m_statsModification = PersistableObject.Load<ArrowCombatOffense.Modification>(reader);

        if (reader.ReadBool())
        {
            Ignite();
        }
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
            StopBeingDangerous(randomAngularVelocity: true);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        StopBeingDangerous(randomAngularVelocity: false);
    }
    protected virtual void StopBeingDangerous(bool randomAngularVelocity)
    {
        if (m_isNeutralized)
            return;

        m_isNeutralized = true;

        // Let fire effect finish
        FireAttachment.DetachAndDestroyWhenParticlesFinish();

        // Destroy self
        Destroy(gameObject);

        // Spawn remains
        var remains = m_data.remainsSpawner?.Spawn(
            position: transform.position,
            rotation: transform.rotation,
            velocity: m_rigidbody2D.velocity,
            angularVelocity:
                randomAngularVelocity
                    ? RandomFinishingAngularVelocity()
                    : m_rigidbody2D.angularVelocity);

        if (m_isIgnited)
        {
            remains.Ignite();
        }
    }

    float RandomFinishingAngularVelocity()
    {
        bool direction = Random.value > 0.5f;
        return (direction ? 1 : -1) * Random.Range(150, 360) * m_rigidbody2D.velocity.magnitude / 5;
    }

    static Arrow()
    {
        PersistableComponent.Register<Arrow>("Arrow");
    }
}
