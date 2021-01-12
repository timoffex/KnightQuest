using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component for GameObjects that can be recreated from a prefab.
/// </summary>
public sealed class PersistablePrefab : MonoBehaviour
{
    public string PrefabId => prefabId;
    public string SceneId => sceneId.Length > 0 ? sceneId : null;

    [SerializeField]
    [Tooltip("Reference to the prefab to which this component is attached. Set this on the prefab"
            + " and never change it!")]
    string prefabId;

    [SerializeField]
    [Tooltip("A unique ID for this object if it is in a scene. This should be empty on prefabs"
            + " but set to a unique value on scene objects.")]
    string sceneId;

    GameSingletons m_gameSingletons;

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
    /// Adds a child object that should be persisted.
    /// </summary>
    public void Add(PersistablePrefab subobject)
    {
        m_subobjects.Add(subobject);
    }

    public void Save(GameDataWriter writer)
    {
        // Code corresponding to LoadPrefab()
        writer.WriteString(prefabId);
        if (sceneId != null)
        {
            writer.WriteBool(true);
            writer.WriteString(sceneId);
        }
        else
        {
            writer.WriteBool(false);
        }

        // Code corresponding to Load()

        // Unity components
        SaveUnityComponents(writer);

        // Persistable components on the same object
        writer.WriteInt16((short)m_components.Count);
        foreach (var obj in m_components)
        {
            obj.Save(writer);
        }

        // Persistable child objects
        writer.WriteInt16((short)m_subobjects.Count);
        foreach (var obj in m_subobjects)
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

        // Persistable child objects
        var numChildren = reader.ReadInt16();
        for (int i = 0; i < numChildren; ++i)
        {
            var loaded = LoadPrefab(reader);
            loaded.transform.parent = transform;
            m_subobjects.Add(loaded);
        }
    }

    public static PersistablePrefab LoadPrefab(GameDataReader reader)
    {
        PersistablePrefab persistable = null;
        var prefabId = reader.ReadString();

        string sceneId = null;
        if (reader.ReadBool())
        {
            sceneId = reader.ReadString();
            persistable = GameSingletons.Instance.GetPersistableBySceneId(sceneId);
        }

        if (persistable == null)
        {
            var prefab = GameSingletons.Instance.PrefabCollection.GetPrefabById(prefabId);
            persistable = Instantiate(prefab);
            persistable.sceneId = sceneId;
        }

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

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;

        if (transform.parent == null)
        {
            m_gameSingletons.AddRootPersistableObject(this);
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
                m_gameSingletons.AddNonRootPersistableObject(this);
            }
        }
    }

    void OnDestroy()
    {
        m_gameSingletons.RemovePersistableObject(this);
    }
}
