using UnityEngine;

/// <summary>
/// A simple <see cref="CombatOffense"/> that "applies damage", for some definition of doing that.
/// </summary>
public sealed class SimpleDamageOffense : CombatOffense
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

    static SimpleDamageOffense()
    {
        PersistableComponent.Register<SimpleDamageOffense>("SimpleDamageOffense");
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
            combatStats.TakeDirectDamage(damage);
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
                "SimpleDamageOffense.Modification",
                Loader);
        }
    }
}
