using UnityEngine;

/// Invokes <see cref="Sword.OnCollidedWith(UnityEngine.Collider2D)"/>  on a parent object
/// when anything enters the trigger attached to this object.
[RequireComponent(typeof(Collider2D))]
public class SwordCollider : MonoBehaviour
{
    Sword m_swordAttack;

    void Start()
    {
        m_swordAttack = GetComponentInParent<Sword>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        m_swordAttack.OnCollidedWith(collider);
    }
}
