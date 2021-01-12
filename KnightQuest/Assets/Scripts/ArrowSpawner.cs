using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Spawner for <see cref="Arrow"/>.
[CreateAssetMenu(menuName = "Game/Arrow Spawner")]
public class ArrowSpawner : ScriptableObject
{
    [SerializeField] Arrow prefab;

    public Arrow Spawn(
        GameObject attacker,
        Vector2 position,
        Vector2 velocity,
        float liveTime,
        CombatStatsModifier statsModifier)
    {
        Arrow arrow = Instantiate(prefab, position, Quaternion.identity);
        arrow.Initialize(attacker, velocity, liveTime, statsModifier);
        return arrow;
    }
}
