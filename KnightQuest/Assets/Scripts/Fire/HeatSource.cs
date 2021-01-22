using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class HeatSource : MonoBehaviour
{
    GameSingletons m_gameSingletons;
    bool m_started;

    /// <summary>
    /// Heats the <paramref name="receiver"/>. Called in Update().
    /// </summary>
    public abstract void Heat(HeatReceiver receiver);

    protected virtual void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
        m_started = true;
    }

    protected virtual void OnTriggerStay2D(Collider2D collider)
    {
        // Disabled components still receive trigger events
        if (!enabled)
            return;
        // Even components that haven't started receive these events
        if (!m_started)
            return;

        m_gameSingletons.FireSystem.AddHeat(collider.gameObject, this);
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (!TryGetComponent<Collider2D>(out var collider))
        {
            Debug.LogWarning(
                $"No trigger collider present on {gameObject} heat source", gameObject);
        }
        else if (!collider.isTrigger)
        {
            Debug.LogWarning($"Changed collider on {gameObject} to a trigger collider", gameObject);
            collider.isTrigger = true;
        }
    }
#endif
}
