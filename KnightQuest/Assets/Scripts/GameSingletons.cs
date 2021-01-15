using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singletons that are available at all times when in the main part of the game.
/// </summary>
/// <remarks>
/// In Haskell, everything accessible here would be in the Reader environment used by the game 
/// object generators. A Reader environment is like a collection of implicit global parameters.
/// 
/// Unity doesn't allow objects to have constructors, but implicit global parameters are just like
/// singletons. To stay true to the spirit of this class, capture the singletons in Start() by using
/// <see cref="Instance"/>. Avoid using <see cref="Instance"/> at any time other than object
/// construction.
/// 
/// A <see cref="GameSingletons"/> object should be created when the user starts playing the game
/// (for example after loading a game from the main menu). The object should remain alive until
/// the user exits the main mode of the game.
/// </remarks>
public sealed class GameSingletons : MonoBehaviour
{
    #region Singleton
    public static GameSingletons Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
            Debug.LogError($"GameSingletons instance already exists: {Instance}", Instance);
        else
            Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    [SerializeField] Camera mainCamera;

    [SerializeField] PersistablePrefabCollection prefabCollection;

    public PersistablePrefabCollection PrefabCollection => prefabCollection;

    public Vector2 MouseWorldPosition =>
        mainCamera.ScreenToWorldPoint(Input.mousePosition);

    /// <summary>
    /// The <see cref="Character"/> the player is currently playing. May be null.
    /// </summary>
    public Character PlayerCharacter { get; set; }

    public void AddRootPersistableObject(PersistablePrefab persistableObject)
    {
        if (persistableObject.SceneId != null)
        {
            m_sceneIdToPersistableObject.Add(persistableObject.SceneId, persistableObject);
        }

        m_rootPersistableObjects.Add(persistableObject);
    }

    public void AddNonRootPersistableObject(PersistablePrefab persistableObject)
    {
        Debug.Assert(persistableObject.SceneId != null);
        m_sceneIdToPersistableObject.Add(persistableObject.SceneId, persistableObject);
    }

    public void RemovePersistableObject(PersistablePrefab persistableObject)
    {
        if (persistableObject.SceneId != null)
        {
            m_sceneIdToPersistableObject.Remove(persistableObject.SceneId);
        }

        m_rootPersistableObjects.Remove(persistableObject);
    }

    public PersistablePrefab GetPersistableBySceneId(string sceneId)
    {
        if (m_sceneIdToPersistableObject.TryGetValue(sceneId, out var obj))
        {
            return obj;
        }
        return null;
    }

    public void SaveTo(GameDataWriter writer)
    {
        writer.WriteInt16((short)m_rootPersistableObjects.Count);
        foreach (var obj in m_rootPersistableObjects)
        {
            obj.Save(writer);
        }
    }

    public void LoadFrom(GameDataReader reader)
    {
        // Remove all current root persistable objects that don't have a scene ID
        foreach (var rootObject in m_rootPersistableObjects)
        {
            if (rootObject.SceneId == null)
                Destroy(rootObject.gameObject);
        }

        var numObjects = reader.ReadInt16();
        m_rootPersistableObjects.Clear();
        for (int i = 0; i < numObjects; ++i)
        {
            PersistablePrefab.LoadPrefab(reader);
        }
    }

    readonly HashSet<PersistablePrefab> m_rootPersistableObjects
        = new HashSet<PersistablePrefab>();
    readonly Dictionary<string, PersistablePrefab> m_sceneIdToPersistableObject
        = new Dictionary<string, PersistablePrefab>();
}
