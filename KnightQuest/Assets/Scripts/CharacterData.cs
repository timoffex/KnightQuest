using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class CharacterData : MonoBehaviour
{
    [Tooltip("Maximum speed in units / second.")]
    public float maxSpeed;

    [Tooltip("The center of the character around which weapons rotate.")]
    public Transform weaponCenterPoint;

    [Tooltip("The distance from the center point at which weapons should appear.")]
    public float weaponRadius;

    [Tooltip("The amount of force to apply to move in a desired direction.")]
    public float movementForce = 1;
}
