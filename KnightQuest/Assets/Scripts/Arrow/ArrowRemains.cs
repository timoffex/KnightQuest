using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowRemains : PersistableComponent, IIgnitable
{
    Rigidbody2D m_rigidbody;

    float m_deathTime;
    bool m_isIgnited;

    /// <summary>
    /// The child fire controller. This should only be set by the
    /// <see cref="ArrowRemainsFireAttachment"/> class.
    /// </summary>
    public ArrowRemainsFireAttachment FireAttachment { private get; set; }

    protected float TimeToLive
    {
        get => m_deathTime - Time.time;
        set
        {
            m_deathTime = Time.time + value;
        }
    }

    public virtual void Initialize(
        Vector2 velocity, float angularVelocity)
    {
        m_rigidbody.velocity = velocity;
        m_rigidbody.angularVelocity = angularVelocity;
        TimeToLive = 5f;
    }

    public void Ignite()
    {
        FireAttachment.Ignite();
        m_isIgnited = true;
    }

    protected override void Awake()
    {
        base.Awake();
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (TimeToLive <= 0)
            Destroy(gameObject);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteFloat(TimeToLive);
        writer.WriteBool(m_isIgnited);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        TimeToLive = reader.ReadFloat();
        if (reader.ReadBool())
            Ignite();
    }

    static ArrowRemains()
    {
        PersistableComponent.Register<ArrowRemains>("ArrowRemains");
    }
}
