﻿using UnityEngine;

/// <summary>
/// A simple stats modifier that "applies damage", for some definition of doing that.
/// </summary>
public sealed class SimpleDamageStatsModifier : CombatStatsModifier
{
    [SerializeField] float damage;

    public override CombatStatsModifier.Modification Value => new Modification(damage);

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

    new public sealed class Modification : CombatStatsModifier.Modification
    {
        readonly float damage;

        public Modification(float damage)
        {
            this.damage = damage;
        }

        public override void Modify(CombatStats combatStats)
        {
            combatStats.TakeDirectDamage(damage);
        }

        public override CombatStatsModifier.Modification WithDamageMultiplier(float multiplier)
        {
            return new Modification(damage * multiplier);
        }

        public override void Save(GameDataWriter writer)
        {
            base.Save(writer);
            writer.WriteFloat(damage);
        }

        static Modification Loader(GameDataReader reader) =>
            new Modification(reader.ReadFloat());

        static Modification()
        {
            PersistableObject.Register<Modification>(
                "SimpleDamageStatsModifier.Modification",
                Loader);
        }
    }
}
