using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAttack : Weapon
{
    [SerializeField] ArrowSpawner arrowSpawner;

    [SerializeField] float arrowSpeed;

    protected override void Attack()
    {
        arrowSpawner.Spawn(transform.position, Direction * arrowSpeed);
    }
}
