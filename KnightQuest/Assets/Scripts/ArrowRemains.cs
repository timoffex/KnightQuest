using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ArrowRemains : PersistableComponent
{
    Rigidbody2D m_rigidbody;

    float m_deathTime;

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
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        TimeToLive = reader.ReadFloat();
    }

    static ArrowRemains()
    {
        PersistableComponent.Register<ArrowRemains>("ArrowRemains");
    }
}
