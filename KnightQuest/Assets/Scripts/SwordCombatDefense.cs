public sealed class SwordCombatDefense : CombatDefense
{
    public override void TakeSwordDamage(CombatStats combatStats, float damage)
    {
        // Damage from other swords is reduced to represent the fact that the user can parry
        // attacks.
        combatStats.TakeDirectDamage(0.2f * damage);
    }

    static SwordCombatDefense()
    {
        PersistableComponent.Register<SwordCombatDefense>("SwordCombatDefense");
    }
}
