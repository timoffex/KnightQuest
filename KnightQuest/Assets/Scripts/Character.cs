using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    [Tooltip("Maximum speed in units / second.")]
    public float maxSpeed;

    [Tooltip("The center of the character around which weapons rotate.")]
    [SerializeField]
    Transform weaponCenterPoint;

    [Tooltip("The distance from the center point at which weapons should appear.")]
    [SerializeField]
    float weaponRadius;

    public CharacterDirection Direction { get; private set; }

    public Transform WeaponCenterPoint => weaponCenterPoint;

    public float WeaponRadius => weaponRadius;

    Rigidbody2D m_rigidbody2D;
    GameSingletons m_gameSingletons;
    float m_freezeDirectionUntilTime;

    /// Momentarily freezes Direction for attack animations.
    public void AttackFreezeFrame(Vector2 direction)
    {
        SetLookDirection(direction);
        m_freezeDirectionUntilTime = Time.time + 0.3f;
    }

    void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
    }

    void Update()
    {
        if (Time.time > m_freezeDirectionUntilTime)
        {
            SetLookDirection(m_rigidbody2D.velocity);
        }
    }

    void SetLookDirection(Vector2 dir)
    {
        if (Mathf.Approximately(dir.sqrMagnitude, 0))
            return;

        Direction = CharacterDirectionUtils.CharacterDirectionFromNonzero(dir);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(weaponCenterPoint.position, weaponRadius);
    }
}