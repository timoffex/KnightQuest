public sealed class NoCombatDefense : CombatDefense
{
    static NoCombatDefense()
    {
        PersistableComponent.Register<NoCombatDefense>("NoCombatDefense");
    }
}
