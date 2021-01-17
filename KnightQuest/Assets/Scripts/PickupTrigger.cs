using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An attachment to a <see cref="Pickup"/> that activates it when a player walks into a trigger
/// collider.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public sealed class PickupTrigger : MonoBehaviour
{
    Pickup m_pickup;

    void Start()
    {
        m_pickup = GetComponentInParent<Pickup>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();

        if (player != null)
        {
            m_pickup.GetPickedUpBy(player);
        }
    }
}
