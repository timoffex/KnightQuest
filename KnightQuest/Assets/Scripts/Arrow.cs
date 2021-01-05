using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controller script for an arrow that can hit <see cref="Attackable"/> instances.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Arrow : MonoBehaviour
{
    [SerializeField] float liveTime = 2;

    Rigidbody2D m_rigidbody2D;
    Collider2D m_collider2D;

    float deathTime;

    public void Initialize(Vector2 velocity)
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, velocity);
        m_rigidbody2D.velocity = velocity;

        deathTime = Time.time + liveTime;
    }

    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_collider2D = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (Time.time > deathTime)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var attackable = collider.GetComponent<Attackable>();
        if (attackable != null)
        {
            attackable.OnHit(m_rigidbody2D.velocity * m_rigidbody2D.mass);
            Destroy(gameObject);
        }
    }
}
