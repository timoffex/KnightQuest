using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class CombatStats : PersistableComponent
{
    [SerializeField] float baseMaximumHealth;

    [SerializeField] float currentHealth;

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
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        currentHealth = reader.ReadFloat();
    }
}
