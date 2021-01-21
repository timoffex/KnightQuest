using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(RadialHeatSourceData))]
public class RadialHeatSource : HeatSource
{
    RadialHeatSourceData m_radialHeatSourceData;
    CircleCollider2D m_collider;

    public override void Heat(HeatReceiver receiver)
    {
        var outerRadius = m_collider.radius;
        var innerRadius = m_radialHeatSourceData.constantHeatRadiusRatio * outerRadius;
        var distance = receiver.GetDistanceTo((Vector2)transform.position + m_collider.offset);

        float scale = 1 - Mathf.Clamp01((distance - innerRadius) / (outerRadius - innerRadius));

        if (scale > 0)
        {
            receiver.AddTemperatureFromHeatSource(m_radialHeatSourceData.centerHeat * scale);
        }
    }

    protected virtual void Awake()
    {
        m_radialHeatSourceData = GetComponent<RadialHeatSourceData>();
        m_collider = GetComponent<CircleCollider2D>();
    }

#if UNITY_EDITOR
    protected virtual void OnDrawGizmos()
    {
        if (m_radialHeatSourceData == null &&
            !TryGetComponent<RadialHeatSourceData>(out m_radialHeatSourceData))
        {
            return;
        }

        if (m_collider == null &&
            !TryGetComponent<CircleCollider2D>(out m_collider))
        {
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,
            m_radialHeatSourceData.constantHeatRadiusRatio * m_collider.radius);
    }
#endif
}
