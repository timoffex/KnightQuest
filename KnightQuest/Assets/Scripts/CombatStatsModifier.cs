using UnityEngine;

public abstract class CombatStatsModifier : PersistableComponent
{
    public abstract void Modify(CombatStats combatStats);
}