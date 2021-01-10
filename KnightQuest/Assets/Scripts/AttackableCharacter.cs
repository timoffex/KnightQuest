using UnityEngine;

public class AttackableCharacter : Attackable
{
    Rigidbody2D m_rigidbody2D;
    Character m_character;

    public override void OnHit(Vector2 impactImpulse, CombatStatsModifier statsModifier)
    {
        base.OnHit(impactImpulse, statsModifier);
        m_rigidbody2D.AddForce(impactImpulse, ForceMode2D.Impulse);
        m_character.ApplyStatsModifier(statsModifier);
    }

    protected virtual void Start()
    {
        m_rigidbody2D = GetComponentInParent<Rigidbody2D>();
        m_character = GetComponentInParent<Character>();
    }
}