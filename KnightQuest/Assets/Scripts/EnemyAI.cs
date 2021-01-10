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

    Path m_pathToTarget;
    int m_currentWaypoint;
    float m_nextPathfindingTime;

    protected Transform GroundPoint => m_data.groundPoint;

    protected float MinimumDistanceToWaypoint => m_data.minimumDistanceToWaypoint;

    protected virtual void Start()
    {
        m_data = GetComponent<EnemyAIData>();
        m_seeker = GetComponent<Seeker>();
    }

    protected virtual void Update()
    {
        UpdatePath();

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

    void UpdatePath()
    {
        if (Time.time < m_nextPathfindingTime || m_data.targetGroundPoint == null)
            return;

        m_seeker.StartPath(
            m_data.groundPoint.position,
            m_data.targetGroundPoint.position,
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
