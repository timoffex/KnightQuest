using UnityEngine;

public sealed class HealthBar : MonoBehaviour
{
    [SerializeField] RectTransform fillImage;

    CombatStats m_stats;
    RectTransform m_fullRect;

    void Start()
    {
        m_stats = GetComponentInParent<CombatStats>();
        m_fullRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        float percentHealth = m_stats.CurrentHealth / m_stats.MaximumHealth;

        fillImage.offsetMax =
            new Vector2(
                -m_fullRect.rect.width * (1 - Mathf.Clamp01(percentHealth)),
                0);
    }
}
