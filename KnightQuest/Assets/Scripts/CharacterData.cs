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
}
