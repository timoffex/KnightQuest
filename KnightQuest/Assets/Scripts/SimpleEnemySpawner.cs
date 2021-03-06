﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SimpleEnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] weaponPrefabs;
    [SerializeField] PersistablePrefab lootPrefab;
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

        if (weaponPrefabs.Length > 0)
        {
            var weaponPrefab = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];
            Instantiate(weaponPrefab, enemy.transform);
        }

        if (lootPrefab != null)
        {
            var lootDropper = enemy.AddComponent<CharacterLootDropper>();
            lootDropper.Initialize(lootPrefab);
        }
    }
}
