using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Arrow Remains Spawner")]
public class ArrowRemainsSpawner : ScriptableObject
{
    [SerializeField] ArrowRemains remainsPrefab;
    [SerializeField] GameObject particleEffect;

    public virtual ArrowRemains Spawn(
        Vector2 position, Quaternion rotation, Vector2 velocity, float angularVelocity)
    {
        var remains = Instantiate(remainsPrefab, position, rotation);
        remains.Initialize(velocity, angularVelocity);

        Instantiate(particleEffect, position, rotation);

        return remains;
    }
}
