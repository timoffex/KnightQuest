using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AttackableCharacter : Attackable
{
    Rigidbody2D m_rigidbody2D;

    public override void OnHit(Vector2 impactImpulse)
    {
        base.OnHit(impactImpulse);
        m_rigidbody2D.AddForce(impactImpulse, ForceMode2D.Impulse);
    }

    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }
}