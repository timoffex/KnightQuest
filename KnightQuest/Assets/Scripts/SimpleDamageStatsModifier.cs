using UnityEngine;

/// <summary>
/// A simple stats modifier that "applies damage", for some definition of doing that.
/// </summary>
public sealed class SimpleDamageStatsModifier : CombatStatsModifier
{
    [SerializeField] float damage;

    public override void Modify(CombatStats combatStats)
    {
        combatStats.TakeDirectDamage(damage);
    }

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteFloat(damage);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        damage = reader.ReadFloat();
    }
}
