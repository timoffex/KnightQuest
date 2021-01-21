/// <summary>
/// A component that defines how an entity takes different kinds of damage.
/// </summary>
public abstract class CombatDefense : PersistableComponent
{
    public virtual void TakeSwordDamage(
        CombatStats combatStats,
        float damage)
    {
        combatStats.TakeDirectDamage(damage);
    }

    public virtual void TakeArrowDamage(
        CombatStats combatStats,
        float damage)
    {
        combatStats.TakeDirectDamage(damage);
    }

    public virtual void TakeFireDamage(
        CombatStats combatStats,
        float damage)
    {
        combatStats.TakeDirectDamage(damage);
    }
}