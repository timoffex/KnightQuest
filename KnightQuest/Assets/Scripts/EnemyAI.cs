using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(EnemyAIData))]
public abstract class EnemyAI : MonoBehaviour
{
    EnemyAIData m_data;
    Seeker m_seeker;
    GameSingletons m_gameSingletons;

    Path m_pathToTarget;
    int m_currentWaypoint;
    float m_nextPathfindingTime;
    Character m_target;

    public Vector2 TargetPosition => m_target.GroundPoint.position;

    public float DistanceToTarget => Vector2.Distance(GroundPoint.position, TargetPosition);

    public bool HasTarget => m_target != null;

    protected abstract Transform GroundPoint { get; }

    protected float MinimumDistanceToWaypoint => m_data.minimumDistanceToWaypoint;

    public void MoveTowardTarget()
    {
        if (m_pathToTarget != null)
        {
            m_currentWaypoint =
                FollowPath(m_pathToTarget, m_currentWaypoint);
            if (m_currentWaypoint >= m_pathToTarget.vectorPath.Count)
            {
                m_pathToTarget = null;
            }
        }
    }

    protected virtual void Start()
    {
        m_data = GetComponent<EnemyAIData>();
        m_seeker = GetComponent<Seeker>();

        m_gameSingletons = GameSingletons.Instance;
    }

    protected virtual void Update()
    {
        m_target = m_gameSingletons.PlayerCharacter;
        UpdatePath();
    }

    void UpdatePath()
    {
        if (!HasTarget)
        {
            m_pathToTarget = null;
            return;
        }

        if (Time.time < m_nextPathfindingTime)
            return;

        m_seeker.StartPath(
            GroundPoint.position,
            TargetPosition,
            OnPathCallback);
        m_nextPathfindingTime = Time.time + m_data.pathfindingDelay;
    }

    void OnPathCallback(Path path)
    {
        if (path.error)
        {
            Debug.LogError($"Pathfinding error: {path.errorLog}");
            return;
        }

        m_pathToTarget = path;
        m_currentWaypoint = 0;
    }

    /// <summary>
    /// Move along the path, returning the index of the next waypoint to follow.
    /// </summary>
    protected abstract int FollowPath(Path path, int waypoint);
}
