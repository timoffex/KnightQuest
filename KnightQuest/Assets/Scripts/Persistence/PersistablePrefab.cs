using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component for GameObjects that can be recreated from a prefab.
/// </summary>
public sealed class PersistablePrefab : MonoBehaviour
{
    public string PrefabId => prefabId;

    public ISet<PersistablePrefab> Subobjects => m_subobjects;

    [SerializeField]
    [Tooltip("Reference to the prefab to which this component is attached. Set this on the prefab"
            + " and never change it!")]
    string prefabId;

    GameData m_gameData;

    /// <summary>
    /// The set of registered persistable components.
    /// </summary>
    readonly HashSet<PersistableComponent> m_components = new HashSet<PersistableComponent>();

    /// <summary>
    /// The set of registered subobjects.
    /// </summary>
    readonly HashSet<PersistablePrefab> m_subobjects = new HashSet<PersistablePrefab>();


    /// <summary>
    /// Adds a component from the same object that should be persisted.
    /// </summary>
    public void Add(PersistableComponent persistable)
    {
        m_components.Add(persistable);
    }

    /// <summary>
    /// Registers a persisted child object.
    /// </summary>
    public void Add(PersistablePrefab subobject)
    {
        m_subobjects.Add(subobject);
    }

    public void Save(GameDataWriter writer)
    {
        // Unity components
        SaveUnityComponents(writer);

        // Persistable components on the same object
        writer.WriteInt16((short)m_components.Count);
        foreach (var obj in m_components)
        {
            obj.Save(writer);
        }
    }

    void Load(GameDataReader reader)
    {
        // Unity components
        LoadUnityComponents(reader);

        // Persistable components on the same object
        var numComponents = reader.ReadInt16();
        for (int i = 0; i < numComponents; ++i)
        {
            m_components.Add(PersistableComponent.LoadOn(reader, gameObject));
        }
    }

    public static PersistablePrefab InstantiatePrefab(
        string prefabId, GameDataReader reader, PersistablePrefab parent = null)
    {
        var persistable =
            Instantiate(
                GameSingletons.Instance.PrefabCollection.GetPrefabById(prefabId),
                parent?.transform);
        persistable.Load(reader);
        return persistable;
    }

    void SaveUnityComponents(GameDataWriter writer)
    {
        // Transform
        writer.WriteVector3(transform.position);
        writer.WriteQuaternion(transform.rotation);

        // Physics
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            if (rigidbody == null)
            {
                writer.WriteBool(false);
            }
            else
            {
                writer.WriteBool(true);
                writer.WriteVector3(rigidbody.velocity);
            }
        }
    }

    void LoadUnityComponents(GameDataReader reader)
    {
        // Transform
        transform.position = reader.ReadVector3();
        transform.rotation = reader.ReadQuaternion();

        // Physics
        {
            if (reader.ReadBool())
            {
                var rigidbody = GetComponent<Rigidbody2D>();
                if (rigidbody == null) rigidbody = gameObject.AddComponent<Rigidbody2D>();
                rigidbody.velocity = reader.ReadVector3();
            }
        }
    }

    void OnEnable()
    {
        m_gameData = GameSingletons.Instance.GameData;

        if (transform.parent == null)
        {
            m_gameData.AddRootObjectToCurrentScene(this);
        }
        else
        {
            var parent = transform.parent.GetComponent<PersistablePrefab>();
            if (parent == null)
            {
                Debug.LogError(
                        "Every persistable object must either be at the root level or have a"
                        + $" persistable parent. {gameObject.name} will not be persisted.",
                    gameObject);
            }
            else
            {
                parent.Add(this);
            }
        }
    }

    void OnDisable()
    {
        m_gameData.RemoveRootObjectFromCurrentScene(this);
    }
}
