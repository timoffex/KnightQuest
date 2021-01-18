using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// A map between components that can be persisted and non-changing IDs.
/// </summary>
public static class PersistableComponents
{
    public static string GetId(Type type)
    {
        try
        {
            return m_typeToId[type];
        }
        catch (KeyNotFoundException e)
        {
            throw new System.InvalidOperationException(
                $"Type {type} is not registered as a persistable component!", e);
        }
    }

    public static PersistableComponent GetOrAddComponent(string id, GameObject gameObject)
    {
        var type = m_idToType[id];
        var existing = gameObject.GetComponent(type);
        if (existing != null)
            return (PersistableComponent)existing;
        return (PersistableComponent)gameObject.AddComponent(type);
    }

    static PersistableComponents()
    {
        // Don't use nameof because these strings shouldn't change.

        Register<Character>("Character");
        Register<CombatStats>("CombatStats");
        Register<NoCombatDefense>("NoCombatDefense");
        Register<SwordCombatDefense>("SwordCombatDefense");

        Register<Arrow>("Arrow");
        Register<ArrowRemains>("ArrowRemains");
        Register<Sword>("Sword");
        Register<SwordData>("SwordData");
        Register<Bow>("Bow");
        Register<SimpleDamageOffense>("SimpleDamageOffense");
        Register<SwordCombatOffense>("SwordCombatOffense");
        Register<ArrowCombatOffense>("ArrowCombatOffense");

        Register<EnemyAI>("EnemyAI");
        Register<EnemyAIData>("EnemyAIData");
        Register<CharacterEnemyAI>("CharacterEnemyAI");
        Register<CharacterLootDropper>("CharacterLootDropper");

        Register<PlayerCharacterController>("PlayerCharacterController");
        Register<Player>("Player");

        Register<TestWeaponSwapper>("TestWeaponSwapper");
    }

    static void Register<T>(string id) where T : PersistableComponent
    {
        m_typeToId.Add(typeof(T), id);
        m_idToType.Add(id, typeof(T));
    }

    readonly static Dictionary<string, Type> m_idToType
        = new Dictionary<string, Type>();
    readonly static Dictionary<Type, string> m_typeToId
        = new Dictionary<Type, string>();
}