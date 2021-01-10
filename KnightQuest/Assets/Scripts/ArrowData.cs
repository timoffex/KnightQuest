using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class ArrowData : MonoBehaviour
{
    [Tooltip("The time after which the arrow stops flying.")]
    public float liveTime = 0.5f;
}
