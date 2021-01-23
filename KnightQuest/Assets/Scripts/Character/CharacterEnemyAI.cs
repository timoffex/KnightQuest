using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class CharacterEnemyAI : EnemyAI
{
    Character m_character;

    protected override Transform GroundPoint => m_character.GroundPoint;

    protected override void Start()
    {
        base.Start();
        m_character = GetComponent<Character>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (HasTarget)
        {
            m_character.CurrentWeapon?.ControlAI(this);
        }
    }

    protected override int FollowPath(Path path, int waypoint)
    {
        while (waypoint < path.vectorPath.Count &&
                Vector2.Distance(GroundPoint.position, path.vectorPath[waypoint])
                    < MinimumDistanceToWaypoint)
            ++waypoint;

        if (waypoint < path.vectorPath.Count)
        {
            var direction = path.vectorPath[waypoint] - GroundPoint.position;
            if (direction.sqrMagnitude > 0)
                m_character.MoveInDirection(direction.normalized);
        }

        return waypoint;
    }

    static CharacterEnemyAI()
    {
        PersistableComponent.Register<CharacterEnemyAI>("CharacterEnemyAI");
    }
}
