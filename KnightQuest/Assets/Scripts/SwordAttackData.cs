using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class SwordAttackData : MonoBehaviour
{
    public float attackStrength = 1;

    public float attackCooldown = 1;

    [Tooltip("Extra attack range to add on top of the character's weapon radius for AIs to"
             + " determine when to swing the sword.")]
    public float extraAttackRange;
}
