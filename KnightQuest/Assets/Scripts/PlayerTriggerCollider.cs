using UnityEngine;

/// <summary>
/// An attachment to a player object for triggering <see cref="PlayerTriggerZone"/>s.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public sealed class PlayerTriggerCollider : MonoBehaviour
{
    public Player Player { get; private set; }

    void Awake()
    {
        Player = GetComponentInParent<Player>();
        if (Player == null)
        {
            Debug.LogError("PlayerTriggerCollider not attached to a Player object", gameObject);
        }
    }
}
