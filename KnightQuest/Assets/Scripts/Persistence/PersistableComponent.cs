using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for components that can be persisted (saved/loaded).
/// 
/// Each concrete type implementing this class must be registered by using <see cref="Register"/>.
/// A GameObject with a component implementing this class should have a
/// <see cref="PersistablePrefab"/> component for data to be persisted.
/// 
/// Persistable components should not be removed from GameObjects at runtime because that would
/// require explicitly persisting the absence of a component.
/// </summary>
[RequireComponent(typeof(PersistablePrefab))]
public abstract class PersistableComponent : MonoBehaviour
{
    /// <summary>
    /// The persistable prefab to which this is attached. May be null (in which case this component
    /// is not saved).
    /// </summary>
    PersistablePrefab m_persistable;

    string PersistableComponentId => GetId(GetType());

    /// <summary>
    /// Saves the data specific to this component to the given writer.
    /// 
    /// Always call the base method first.
    /// </summary>
    public virtual void Save(GameDataWriter writer)
    {
        // Corresponds to LoadOn
        writer.WriteString(PersistableComponentId);

        // Corresponds to Load
        writer.WriteBool(enabled);
    }

    /// <summary>
    /// Loads the data from the reader.
    /// 
    /// Always call the base method first.
    /// </summary>
    public virtual void Load(GameDataReader reader)
    {
        enabled = reader.ReadBool();
    }

    public static PersistableComponent LoadOn(GameDataReader reader, GameObject gameObject)
    {
        var id = reader.ReadString();
        var self = GetOrAddComponent(id, gameObject);
        self.Load(reader);
        return self;
    }

    protected virtual void Awake()
    {
        m_persistable = GetComponent<PersistablePrefab>();
        m_persistable?.Add(this);
    }

    public static void Register<T>(string id) where T : PersistableComponent
    {
        m_typeToId.Add(typeof(T), id);
        m_idToType.Add(id, typeof(T));
    }

    static string GetId(Type type)
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

    static PersistableComponent GetOrAddComponent(string id, GameObject gameObject)
    {
        var type = m_idToType[id];
        var existing = gameObject.GetComponent(type);
        if (existing != null)
            return (PersistableComponent)existing;
        return (PersistableComponent)gameObject.AddComponent(type);
    }

    readonly static Dictionary<string, Type> m_idToType
        = new Dictionary<string, Type>();
    readonly static Dictionary<Type, string> m_typeToId
        = new Dictionary<Type, string>();
}
