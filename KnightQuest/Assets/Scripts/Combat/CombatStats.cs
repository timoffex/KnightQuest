using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class CombatStats : PersistableComponent
{
    [SerializeField] float baseMaximumHealth;

    [SerializeField] float currentHealth;

    [SerializeField] float healthRegen;

    public float CurrentHealth => currentHealth;

    public float MaximumHealth => baseMaximumHealth;

    /// <summary>
    /// Applies damage directly to health.
    /// </summary>
    public void TakeDirectDamage(float amount)
    {
        currentHealth -= amount;
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteFloat(currentHealth);
        writer.WriteFloat(healthRegen);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        currentHealth = reader.ReadFloat();
        healthRegen = reader.ReadFloat();
    }

    void Update()
    {
        currentHealth += healthRegen * Time.deltaTime;
        if (currentHealth >= baseMaximumHealth)
            currentHealth = baseMaximumHealth;
    }

    static CombatStats()
    {
        PersistableComponent.Register<CombatStats>("CombatStats");
    }
}
