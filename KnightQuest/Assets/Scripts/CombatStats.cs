using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public sealed class CombatStats : MonoBehaviour
{
    [SerializeField] float baseMaximumHealth;

    [SerializeField] float currentHealth;

    public float CurrentHealth => currentHealth;

    /// <summary>
    /// Applies damage directly to health.
    /// </summary>
    public void TakeDirectDamage(float amount)
    {
        currentHealth -= amount;
    }
}
