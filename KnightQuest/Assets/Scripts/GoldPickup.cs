using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A pickup that adds gold to the player.
/// </summary>
public class GoldPickup : Pickup
{
    public override void GetPickedUpBy(Player player)
    {
        player.AddGold(1);
        Destroy(gameObject);
    }
}
