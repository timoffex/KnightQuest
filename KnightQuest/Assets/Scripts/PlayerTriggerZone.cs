using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class PlayerTriggerZone : MonoBehaviour
{
    bool m_started;

    protected virtual void Start()
    {
        m_started = true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        // Disabled / unstarted components can receive these events
        if (!enabled || !m_started)
            return;

        var playerCollider = collider.GetComponent<PlayerTriggerCollider>();
        if (playerCollider != null)
        {
            OnPlayerEntered(playerCollider.Player);
        }
    }

    protected abstract void OnPlayerEntered(Player player);
}
