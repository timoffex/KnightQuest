using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object that can be picked up by a <see cref="Player"/>.
/// </summary>
public abstract class Pickup : MonoBehaviour
{
    public abstract void GetPickedUpBy(Player player);
}
