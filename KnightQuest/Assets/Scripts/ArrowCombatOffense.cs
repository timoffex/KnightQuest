﻿using UnityEngine;

public sealed class ArrowCombatOffense : CombatOffense
{
    [SerializeField] float damage;

    public override CombatOffense.Modification Value => new Modification(damage);

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

    static ArrowCombatOffense()
    {
        PersistableComponent.Register<ArrowCombatOffense>("ArrowCombatOffense");
    }

    new sealed class Modification : CombatOffense.Modification
    {
        readonly float damage;

        public Modification(float damage)
        {
            this.damage = damage;
        }

        public override void Modify(CombatStats combatStats, CombatDefense defense)
        {
            defense.TakeArrowDamage(combatStats, damage);
        }

        public override CombatOffense.Modification WithDamageMultiplier(float multiplier)
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
                "ArrowCombatOffense.Modification",
                Loader);
        }
    }
}
