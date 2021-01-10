using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterEnemyAI : EnemyAI
{
    Character m_character;

    protected override void Start()
    {
        base.Start();
        m_character = GetComponent<Character>();
    }

    protected override int FollowPath(Path path, int waypoint)
    {
        while (waypoint < path.vectorPath.Count &&
                Vector2.Distance(GroundPoint.position, path.vectorPath[waypoint])
                    < MinimumDistanceToWaypoint)
            ++waypoint;

        Debug.Log($"Waypoint: {waypoint}");
        if (waypoint < path.vectorPath.Count)
        {
            var direction = path.vectorPath[waypoint] - GroundPoint.position;
            if (direction.sqrMagnitude > 0)
                m_character.MoveInDirection(direction.normalized);
        }

        return waypoint;
    }
}
