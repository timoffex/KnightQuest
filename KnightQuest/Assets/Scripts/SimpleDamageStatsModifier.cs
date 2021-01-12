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
}
