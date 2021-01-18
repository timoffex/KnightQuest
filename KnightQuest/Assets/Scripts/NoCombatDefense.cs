public sealed class NoCombatDefense : CombatDefense
{
    public override void TakeArrowDamage(CombatStats combatStats, float damage)
    {
        combatStats.TakeDirectDamage(damage);
    }

    public override void TakeSwordDamage(CombatStats combatStats, float damage)
    {
        combatStats.TakeDirectDamage(damage);
    }
}
