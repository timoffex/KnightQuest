using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class ArrowData : MonoBehaviour
{
    public float hitImpulseMultiplier = 10;

    [Tooltip("Extra damage from the arrow when it is on fire.")]
    public float fireDamage = 10;

    public ArrowRemainsSpawner remainsSpawner;
}
