﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for components that can be persisted (saved/loaded).
/// 
/// Each concrete type implementing this class must be registered in
/// <see cref="PersistableComponents"/>. A GameObject with a component implementing this class
/// must also have a <see cref="PersistablePrefab"/> component to support reinstantiating the
/// GameObject if it is destroyed at runtime.
/// 
/// Persistable components should not be removed from GameObjects at runtime because that would
/// require explicitly persisting the absence of a component.
/// </summary>
[RequireComponent(typeof(PersistablePrefab))]
public abstract class PersistableComponent : MonoBehaviour
{
    PersistablePrefab m_persistable;

    string PersistableComponentId => PersistableComponents.GetId(GetType());

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
        var self = PersistableComponents.GetOrAddComponent(id, gameObject);
        self.Load(reader);
        return self;
    }

    protected virtual void Awake()
    {
        m_persistable = GetComponent<PersistablePrefab>();
        if (m_persistable == null)
        {
            Debug.LogError(
                "A PersistablePrefab component is required for saving and loading.", this);
        }

        m_persistable?.Add(this);
    }
}