using UnityEngine;

public abstract class CombatStatsModifier : MonoBehaviour
{
    public abstract void Modify(CombatStats combatStats);
}