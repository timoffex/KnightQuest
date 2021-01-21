using UnityEngine;

public abstract class CombatOffense : PersistableComponent
{
    /// <summary>
    /// Returns the stats modification this creates.
    /// 
    /// This can return a new object every time.
    /// </summary>
    public abstract Modification Value { get; }

    /// <summary>
    /// A modification to <see cref="CombatStats"/>.
    /// 
    /// Subclasses should be immutable.
    /// </summary>
    public abstract class Modification : PersistableObject
    {
        public abstract void Modify(CombatStats combatStats, CombatDefense defense);
    }
}
