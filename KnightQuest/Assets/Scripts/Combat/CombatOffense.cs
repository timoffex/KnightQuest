public abstract class CombatOffense : PersistableComponent
{
    /// <summary>
    /// Returns the stats modification this creates.
    /// 
    /// This can return a new object every time.
    /// </summary>
    public abstract Modification Value { get; }

    /// <summary>
    /// A snapshot of a <see cref="CombatOffense"/> that can be passed around (and, for example,
    /// carried by an arrow).
    /// </summary>
    public abstract class Modification : PersistableObject
    {
        public abstract void Attack(ICombatReceiver receiver);
    }
}
