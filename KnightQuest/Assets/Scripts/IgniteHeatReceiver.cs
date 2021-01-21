using UnityEngine;

/// <summary>
/// A heat receiver that causes an object to ignite.
/// </summary>
public sealed class IgniteHeatReceiver : HeatReceiver
{
    [SerializeField] float ignitionTemperature;

    IIgnitable m_ignitable;

    public override float GetDistanceTo(Vector2 position) =>
        Vector2.Distance(transform.position, position);

    public override void AddTemperatureFromHeatSource(float temperature)
    {
        if (temperature > ignitionTemperature)
        {
            m_ignitable.Ignite();
        }
    }

    void Awake()
    {
        m_ignitable = GetComponentInParent<IIgnitable>();
    }
}
