using System;
using UnityEngine;

public sealed class CombatStats : PersistableComponent
{
    [SerializeField] float baseMaximumHealth;

    [SerializeField] float currentHealth;

    [SerializeField] float healthRegen;

    public float CurrentHealth => currentHealth;

    public float MaximumHealth => baseMaximumHealth;

    public bool IsDead { get; private set; }

    bool m_isOnFire;

    public event Action OnDied;

    /// <summary>
    /// Applies damage directly to health.
    /// </summary>
    public void TakeDirectDamage(float amount)
    {
        currentHealth -= amount;
    }

    public void SetOnFire()
    {
        m_isOnFire = true;
    }

    public void StopFire()
    {
        m_isOnFire = false;
    }

    public void Die()
    {
        if (IsDead)
            return;
        IsDead = true;
        OnDied?.Invoke();
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteFloat(currentHealth);
        writer.WriteFloat(healthRegen);
        writer.WriteBool(m_isOnFire);
        writer.WriteBool(IsDead);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        currentHealth = reader.ReadFloat();
        healthRegen = reader.ReadFloat();
        m_isOnFire = reader.ReadBool();
        IsDead = reader.ReadBool();
    }

    void Update()
    {
        if (IsDead)
            return;

        currentHealth += healthRegen * Time.deltaTime;

        // TODO: This should use CombatDefense.TakeFireDamage(). Maybe every CombatStats requires
        // a CombatDefense? But that entails adding/removing CombatDefense components at runtime..
        // and either way right now that component is used to indicate the defense type on a weapon
        // rather than the defense type on a character (or maybe some combination of both).
        //
        // Maybe the right approach is to have a CombatDefense setter? But what if I forget to set
        // it, or what two pieces of code overwrite each other's setting unintentionally?
        if (m_isOnFire)
            currentHealth -= 10 * Time.deltaTime;

        if (currentHealth >= baseMaximumHealth)
            currentHealth = baseMaximumHealth;
        else if (currentHealth <= 0)
            Die();
    }

    static CombatStats()
    {
        PersistableComponent.Register<CombatStats>("CombatStats");
    }
}
