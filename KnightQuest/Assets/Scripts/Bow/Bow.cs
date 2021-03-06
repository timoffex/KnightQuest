﻿using UnityEngine;

[RequireComponent(typeof(BowData))]
[RequireComponent(typeof(ArrowCombatOffense))]
[RequireComponent(typeof(AudioSource))]
public class Bow : Weapon
{
    BowData m_bowData;
    ArrowCombatOffense m_statsModifier;
    Character.SpeedReductionToken m_speedReductionToken;
    BowParticles m_bowParticles;
    AudioSource m_drawAudioSource;

    float m_chargeStartTime = 0;

    bool m_charging = false;

    public float MaximumRange => m_bowData.arrowSpeed * m_bowData.arrowLiveTime;

    public float ChargePercentage =>
        m_charging
            ? Mathf.Clamp01((Time.time - m_chargeStartTime) / m_bowData.chargeTime)
            : 0;

    public override void ControlAI(EnemyAI enemyAi)
    {
        if (enemyAi.DistanceToTarget < 0.8f * MaximumRange)
        {
            AlignToward(enemyAi.TargetPosition);
            if (!m_charging)
            {
                BeginCharging();
            }

            if (ChargePercentage > 0.9f)
            {
                ReleaseArrow();
            }
        }
        else
        {
            StopCharging();
        }

        if (enemyAi.DistanceToTarget > 0.5f * MaximumRange)
        {
            enemyAi.MoveTowardTarget();
        }
    }

    public override void OnAttackButtonDown()
    {
        BeginCharging();
    }

    public override void OnAttackButtonUp()
    {
        ReleaseArrow();
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
    }

    public override void Load(GameDataReader reader)
    {
        StopCharging();
        base.Load(reader);
    }

    protected virtual void Update()
    {
        if (ChargePercentage > 0.05f)
            Character.AttackFreezeFrame(Direction);
    }

    protected override void Awake()
    {
        base.Awake();
        m_bowData = GetComponent<BowData>();
        m_statsModifier = GetComponent<ArrowCombatOffense>();
        m_drawAudioSource = GetComponent<AudioSource>();
    }

    protected virtual void OnEnable()
    {
        Debug.Assert(m_bowParticles == null);
        m_bowParticles = Instantiate(m_bowData.bowParticlesPrefab);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        m_speedReductionToken?.Cancel();

        // Can be null if the scene is being unloaded
        if (m_bowParticles != null)
            Destroy(m_bowParticles.gameObject);
    }

    protected override void Attack()
    {
        m_bowData.arrowSpawner.Spawn(
            Character.gameObject,
            transform.position,
            Direction * m_bowData.arrowSpeed * ChargePercentage,
            m_bowData.arrowLiveTime,
            m_statsModifier.ArrowValue.WithBowChargePercentage(ChargePercentage));

        m_bowData.releaseAudioSource.Play();
    }

    void BeginCharging()
    {
        m_chargeStartTime = Time.time;
        m_speedReductionToken?.Cancel();
        m_speedReductionToken = Character.TemporarilyReduceSpeed();
        m_charging = true;

        m_drawAudioSource.Play();
    }

    void ReleaseArrow()
    {
        Attack();

        if (ChargePercentage > 0.8f)
        {
            m_bowParticles.transform.position = transform.position;
            m_bowParticles.transform.rotation = transform.rotation;
            m_bowParticles.Activate(Character.Velocity);
        }

        StopCharging();
    }

    void StopCharging()
    {
        m_drawAudioSource.Stop();
        m_charging = false;
        m_speedReductionToken?.Cancel();
        m_speedReductionToken = null;
    }

    static Bow()
    {
        PersistableComponent.Register<Bow>("Bow");
    }
}
