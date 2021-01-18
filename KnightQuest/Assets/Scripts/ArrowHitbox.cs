using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attachment to an <see cref="Arrow"/> that detects when it hits an <see cref="Attackable"/>.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public sealed class ArrowHitbox : MonoBehaviour
{
    Arrow m_arrow;

    void Start()
    {
        m_arrow = GetComponentInParent<Arrow>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<Attackable>(out var attackable))
        {
            m_arrow.Hit(attackable);
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!TryGetComponent<Collider2D>(out var collider))
        {
            Debug.LogWarning($"Missing a 2D trigger collider on the arrow hitbox", this);
        }
        else if (!collider.isTrigger)
        {
            Debug.LogWarning($"Changed arrow hitbox collider to be a trigger", this);
            collider.isTrigger = true;
        }
    }
#endif
}
