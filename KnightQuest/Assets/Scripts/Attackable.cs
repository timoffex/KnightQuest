using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Something that can receive hits.
public class Attackable : MonoBehaviour
{
    public virtual void OnHit(Vector2 impactImpulse, CombatStatsModifier statsModifier)
    {
        Debug.Log($"{gameObject} got hit!");
    }
}
