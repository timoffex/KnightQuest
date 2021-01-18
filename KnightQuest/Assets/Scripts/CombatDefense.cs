/// <summary>
/// A component that defines how an entity takes different kinds of damage.
/// </summary>
public abstract class CombatDefense : PersistableComponent
{
    public abstract void TakeSwordDamage(
        CombatStats combatStats,
        float damage);

    public abstract void TakeArrowDamage(
        CombatStats combatStats,
        float damage);
}