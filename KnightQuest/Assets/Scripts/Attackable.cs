﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Something that can receive hits.
public abstract class Attackable : MonoBehaviour
{
    GameObject m_rootGameObject;

    public bool Hit(
        GameObject attacker,
        Vector2 impactImpulse,
        CombatStatsModifier statsModifier)
    {
        if (attacker == m_rootGameObject)
            return false;

        OnHit(impactImpulse, statsModifier);
        return true;
    }

    protected virtual void OnHit(
        Vector2 impactImpulse,
        CombatStatsModifier statsModifier)
    {
        Debug.Log($"{gameObject} got hit!");
    }

    protected virtual void Awake()
    {
        var parent = transform;

        do
        {
            m_rootGameObject = parent.gameObject;
            parent = m_rootGameObject.transform.parent;
        } while (parent != null);
    }
}
