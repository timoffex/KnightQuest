using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class BowAttackData : MonoBehaviour
{
    public ArrowSpawner arrowSpawner;
    public float arrowSpeed;

    [Tooltip("The time it takes to fully draw the bow, in seconds.")]
    public float chargeTime = 1;
}
