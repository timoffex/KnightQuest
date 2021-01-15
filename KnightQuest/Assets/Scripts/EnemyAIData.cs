using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class EnemyAIData : MonoBehaviour
{
    [Tooltip("The middle point between the character's feet, or in general the point that is"
            + " on the ground and in the center of the character in 3D space.")]
    public Transform groundPoint;

    [Tooltip("Delay in seconds between recalculating a path to the target.")]
    public float pathfindingDelay = 1;

    public float minimumDistanceToWaypoint = 0.01f;
}
