using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class BowParticles : MonoBehaviour
{
    ParticleSystem m_particleSystem;
    Rigidbody2D m_rigidbody;

    public void Activate(Vector2 velocity)
    {
        m_particleSystem.Play();
        m_rigidbody.velocity = velocity;
    }

    void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_particleSystem = GetComponent<ParticleSystem>();
    }
}
