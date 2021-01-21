using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackableCharacter : Attackable
{
    AudioSource m_audioSource;
    Rigidbody2D m_rigidbody2D;
    Character m_character;

    protected override void OnHit(
        Vector2 impactImpulse, CombatOffense.Modification statsModification)
    {
        base.OnHit(impactImpulse, statsModification);
        m_rigidbody2D.AddForce(impactImpulse, ForceMode2D.Impulse);
        m_character.ReceiveAttack(statsModification);
        m_character.OnHitFreezeFrame();

        m_audioSource.Play();
    }

    protected override void Awake()
    {
        base.Awake();
        m_audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start()
    {
        m_rigidbody2D = GetComponentInParent<Rigidbody2D>();
        m_character = GetComponentInParent<Character>();
    }
}