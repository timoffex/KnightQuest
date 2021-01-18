using UnityEngine;

sealed class SwordData : PersistableComponent
{
    public float attackStrength = 1;

    public float attackCooldown = 1;

    [Tooltip("Extra attack range to add on top of the character's weapon radius for AIs to"
             + " determine when to swing the sword.")]
    public float extraAttackRange;

    public override void Save(GameDataWriter writer)
    {
        base.Save(writer);
        writer.WriteFloat(attackStrength);
        writer.WriteFloat(attackCooldown);
        writer.WriteFloat(extraAttackRange);
    }

    public override void Load(GameDataReader reader)
    {
        base.Load(reader);
        attackStrength = reader.ReadFloat();
        attackCooldown = reader.ReadFloat();
        extraAttackRange = reader.ReadFloat();
    }
}
