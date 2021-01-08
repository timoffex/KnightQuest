using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Spawner for <see cref="Arrow"/>.
[CreateAssetMenu(menuName = "Game/Arrow Spawner")]
public class ArrowSpawner : ScriptableObject
{
    [SerializeField] Arrow prefab;

    public Arrow Spawn(Vector2 position, Vector2 velocity, CombatStatsModifier statsModifier)
    {
        Arrow arrow = Instantiate(prefab, position, Quaternion.identity);
        arrow.Initialize(velocity, statsModifier);
        return arrow;
    }
}
