using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SimpleEnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] float spawnDelay;

    float m_nextSpawnTime;

    void Start()
    {
        m_nextSpawnTime = Time.time + spawnDelay;
    }

    void Update()
    {
        if (Time.time > m_nextSpawnTime)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        m_nextSpawnTime = Time.time + spawnDelay;

        var enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        // TODO: This is a bad way of doing things.
        var enemyAiData = enemy.AddComponent<EnemyAIData>();
        enemyAiData.pathfindingDelay = 1;
        enemyAiData.minimumDistanceToWaypoint = 0.05f;
        enemy.AddComponent<CharacterEnemyAI>();

        if (weaponPrefab != null)
        {
            Instantiate(weaponPrefab, enemy.transform);
        }
    }
}
