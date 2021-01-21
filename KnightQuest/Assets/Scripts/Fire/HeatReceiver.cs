using UnityEngine;

// Collider required because heat system uses triggers
[RequireComponent(typeof(Collider2D))]
public abstract class HeatReceiver : MonoBehaviour
{
    public virtual float GetDistanceTo(Vector2 position) =>
        Vector2.Distance(position, transform.position);

    /// <summary>
    /// Registers the given temperature from a heat source. Call in Update().
    /// </summary>
    public abstract void AddTemperatureFromHeatSource(float temperature);
}
