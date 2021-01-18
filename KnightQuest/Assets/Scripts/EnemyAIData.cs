using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class EnemyAIData : PersistableComponent
{
    [Tooltip("Delay in seconds between recalculating a path to the target.")]
    public float pathfindingDelay = 1;

    public float minimumDistanceToWaypoint = 0.01f;

    static EnemyAIData()
    {
        PersistableComponent.Register<EnemyAIData>("EnemyAIData");
    }
}
