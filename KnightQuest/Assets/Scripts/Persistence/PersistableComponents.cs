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
        Register<Character>("Character");
        Register<CombatStats>("CombatStats");
        Register<Arrow>("Arrow");
        Register<SwordAttack>("SwordAttack");
        Register<BowAttack>("BowAttack");
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