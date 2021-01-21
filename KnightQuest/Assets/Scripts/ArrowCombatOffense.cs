using UnityEngine;

public sealed class ArrowCombatOffense : CombatOffense
{
    [SerializeField] float damage;

    public override CombatOffense.Modification Value => ArrowValue;

    public Modification ArrowValue => new Modification(damage, 0);

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

    new public sealed class Modification : CombatOffense.Modification
    {
        readonly float damage;
        readonly float fireDamage;

        public Modification(float damage, float fireDamage)
        {
            this.damage = damage;
            this.fireDamage = fireDamage;
        }

        public override void Modify(CombatStats combatStats, CombatDefense defense)
        {
            defense.TakeArrowDamage(combatStats, damage);
            if (fireDamage > 0)
            {
                defense.TakeFireDamage(combatStats, fireDamage);
            }
        }

        /// <summary>
        /// Adjusts combat stats based on arrow speed percentage.
        /// </summary>
        public Modification WithSpeedMultiplier(float multiplier)
        {
            return new Modification(damage * multiplier, fireDamage);
        }

        /// <summary>
        /// Adjusts combat stats based on how much a bow was charged.
        /// </summary>
        public Modification WithBowChargePercentage(float percentage)
        {
            return new Modification(damage * percentage, fireDamage);
        }

        public Modification WithFireDamage(float fireDamage)
        {
            return new Modification(damage, fireDamage);
        }

        public override void Save(GameDataWriter writer)
        {
            base.Save(writer);
            writer.WriteFloat(damage);
            writer.WriteFloat(fireDamage);
        }

        static Modification Loader(GameDataReader reader) =>
            new Modification(reader.ReadFloat(), reader.ReadFloat());

        static Modification()
        {
            PersistableObject.Register<Modification>(
                "ArrowCombatOffense.Modification",
                Loader);
        }
    }
}
