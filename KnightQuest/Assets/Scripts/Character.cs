using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CombatStats))]
[RequireComponent(typeof(CharacterAnimationController))]
[RequireComponent(typeof(CharacterData))]
public class Character : PersistableComponent, IIgnitable
{

    public CharacterDirection Direction { get; private set; }

    public Vector2 Velocity => m_rigidbody2D.velocity;

    /// <summary>
    /// The character's maximum speed, for the purpose of animation.
    /// </summary>
    public float MaxSpeed => m_data.maxSpeed;

    public float MovementForce =>
        m_temporarySpeedReductions > 0
            ? 0.5f * m_data.movementForce
            : m_data.movementForce;

    public Transform WeaponCenterPoint => m_data.weaponCenterPoint;

    public float WeaponRadius => m_data.weaponRadius;

    /// <summary>
    /// The point representing the "position" of the character. Never null.
    /// </summary>
    public Transform GroundPoint => m_data.groundPoint;

    public Weapon CurrentWeapon { get; set; }

    public bool IsOnFire { get; private set; }

    /// <summary>
    /// An event that triggers when the character dies, immediately before the GameObject is
    /// destroyed.
    /// </summary>
    public event System.Action OnDied;

    CharacterData m_data;
    CombatStats m_combatStats;
    CombatDefense m_combatDefense;
    Rigidbody2D m_rigidbody2D;
    GameSingletons m_gameSingletons;
    CharacterAnimationController m_characterAnimationController;
    float m_freezeDirectionUntilTime;

    /// <summary>
    /// Number of times <see cref="TemporarilyReduceSpeed"/> has been called and not cancelled.
    /// </summary>
    int m_temporarySpeedReductions = 0;

    /// <summary>
    /// Momentarily freezes <see cref="Direction"/> for attack animations.
    /// </summary>
    public void AttackFreezeFrame(Vector2 direction)
    {
        SetLookDirection(direction);
        m_freezeDirectionUntilTime = Time.time + 0.3f;
    }

    /// <summary>
    /// Momentarily freezes <see cref="Direction"/> for when the character is hit.
    /// </summary>
    public void OnHitFreezeFrame()
    {
        m_freezeDirectionUntilTime = Time.time + 0.3f;
    }

    public void ApplyStatsModifier(CombatOffense.Modification modification)
    {
        modification.Modify(m_combatStats, CurrentWeapon?.CombatDefense ?? m_combatDefense);
        m_characterAnimationController.TakeHit();
    }

    /// <summary>
    /// Temporarily halves the character's movement force and returns an action that can be
    /// used to undo this.
    /// </summary>
    public SpeedReductionToken TemporarilyReduceSpeed()
    {
        return new SpeedReductionToken(this);
    }

    public void MoveInDirection(Vector2 normalizedDir)
    {
        m_rigidbody2D.AddForce(
            normalizedDir * MovementForce * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    public void Ignite()
    {
        if (IsOnFire)
            return;

        IsOnFire = true;
        m_characterAnimationController.BeginFireAnimation();
        m_combatStats.SetOnFire();
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteInt16((short)Direction);
        writer.WriteBool(IsOnFire);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        Direction = (CharacterDirection)reader.ReadInt16();
        if (reader.ReadBool())
            Ignite();
    }

    protected override void Awake()
    {
        base.Awake();
        m_data = GetComponent<CharacterData>();
        m_combatStats = GetComponent<CombatStats>();
        m_combatDefense = GetComponent<CombatDefense>() ??
            gameObject.AddComponent<NoCombatDefense>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_characterAnimationController = GetComponent<CharacterAnimationController>();

        m_combatStats.OnDied += DoDeath;

        if (GroundPoint == null)
        {
            Debug.LogError("Character GroundPoint not set", this);
        }
    }

    protected virtual void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
    }

    protected virtual void Update()
    {
        if (Time.time > m_freezeDirectionUntilTime)
        {
            SetLookDirection(m_rigidbody2D.velocity);
        }
    }

    protected virtual void SetLookDirection(Vector2 dir)
    {
        if (Mathf.Approximately(dir.sqrMagnitude, 0))
            return;

        Direction = CharacterDirectionUtils.CharacterDirectionFromNonzero(dir);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (m_data == null)
            m_data = GetComponent<CharacterData>();
        if (m_data == null || WeaponCenterPoint == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(WeaponCenterPoint.position, WeaponRadius);
    }

    void DoDeath()
    {
        Debug.Log($"{gameObject.name} died. Congrats! (Or condolences)");
        OnDied?.Invoke();
        Destroy(gameObject);
    }

    public sealed class SpeedReductionToken
    {
        bool m_didCancel = false;
        readonly Character m_character;

        internal SpeedReductionToken(Character character)
        {
            m_character = character;
            ++m_character.m_temporarySpeedReductions;
        }

        /// <summary>
        /// Undoes the speed reduction.
        /// </summary>
        public void Cancel()
        {
            if (!m_didCancel)
            {
                m_didCancel = true;
                --m_character.m_temporarySpeedReductions;
            }
        }
    }

    static Character()
    {
        PersistableComponent.Register<Character>("Character");
    }
}