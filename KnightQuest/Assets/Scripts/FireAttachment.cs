using System.Collections;
using UnityEngine;

/// <summary>
/// The base class for a controller for a fire attachment to an object.
/// </summary>
/// <remarks>
/// To be honest, I'm not convinced yet that this class is a good idea. It's more like a pair of
/// mixins: one for turning a HeatSource on and off and one for turning a ParticleSystem on and off.
/// It won't work as a base class for any "fire attachment" that doesn't use one of those
/// components.
/// 
/// I could split out a "FireAttachmentMixin" base class later that has particle system and heat
/// source subclasses. The "FireAttachment" would then just use "GetComponents" to get all of these
/// mixins and to call them.
/// </remarks>
[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(HeatSource))]
public abstract class FireAttachment : MonoBehaviour
{
    ParticleSystem m_fireParticles;
    HeatSource m_heatSource;
    bool m_wasIgnited;

    public void Ignite()
    {
        if (m_wasIgnited)
            return;
        m_wasIgnited = true;
        OnIgnited();
    }

    public void Extinguish()
    {
        if (!m_wasIgnited)
            return;
        m_wasIgnited = false;
    }

    public virtual void DetachAndDestroyWhenParticlesFinish()
    {
        transform.parent = null;
        m_fireParticles.Stop();
        StartCoroutine(DestroyAfterParticlesFinishCR());
    }

    protected virtual void OnIgnited()
    {
        m_fireParticles.Play();
        m_heatSource.enabled = true;
    }

    protected virtual void OnExtinguished()
    {
        m_fireParticles.Stop();
        m_heatSource.enabled = false;
    }

    protected virtual void Awake()
    {
        m_fireParticles = GetComponent<ParticleSystem>();
        m_heatSource = GetComponent<HeatSource>();
    }

    IEnumerator DestroyAfterParticlesFinishCR()
    {
        yield return new WaitUntil(() => !m_fireParticles.IsAlive());
        Destroy(gameObject);
    }
}
