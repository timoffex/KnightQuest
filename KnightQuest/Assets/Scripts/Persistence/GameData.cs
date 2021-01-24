using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameData
{
    public void AddRootObjectToCurrentScene(PersistablePrefab rootObject)
    {
        m_currentSceneData.Add(rootObject);
    }

    public void RemoveRootObjectFromCurrentScene(PersistablePrefab rootObject)
    {
        m_currentSceneData.Remove(rootObject);
    }

    /// <summary>
    /// Moves a root object from the current scene to a new scene.
    /// </summary>
    public void MoveToOtherScene(PersistablePrefab rootObject, string sceneName)
    {
        Debug.Assert(!rootObject.HasParent);
        AppendToScene(sceneName, rootObject);
        Object.Destroy(rootObject.gameObject);
    }

    /// <summary>
    /// Saves the current game scene and loads a new one.
    /// </summary>
    public IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Assert(sceneName != null);

        SaveCurrentScene();

        yield return LoadNewSceneWithoutSavingCurrentAsync(sceneName);
    }

    public IEnumerator CloseCurrentSceneAsync()
    {
        Debug.Assert(m_currentScene != null);
        Debug.Assert(m_currentSceneData != null);

        SaveCurrentScene();
        yield return CloseCurrentSceneWithoutSavingAsync();
    }

    public void SaveTo(GameDataWriter writer)
    {
        SaveCurrentScene();

        writer.WriteString(m_currentScene);

        writer.WriteInt16((short)m_initializedScenes.Count);
        foreach (var scene in m_initializedScenes)
        {
            writer.WriteString(scene);
        }

        writer.WriteInt16((short)m_savedSceneData.Count);
        foreach (var entry in m_savedSceneData)
        {
            writer.WriteString(entry.Key);
            writer.WriteInt16((short)entry.Value.Count);
            foreach (var obj in entry.Value)
                obj.SaveTo(writer);
        }
    }

    public IEnumerator LoadFromAndStartAsync(GameDataReader reader)
    {
        // Clear all saved data so that we don't mix old saved data and new saved data.
        m_initializedScenes.Clear();
        m_savedSceneData.Clear();

        var newScene = reader.ReadString();

        var numSavedScenes = reader.ReadInt16();
        foreach (var _ in Enumerable.Range(0, numSavedScenes))
        {
            m_initializedScenes.Add(reader.ReadString());
        }

        var numSceneData = reader.ReadInt16();
        foreach (var _ in Enumerable.Range(0, numSceneData))
        {
            var savedSceneName = reader.ReadString();
            var numSavedObjects = reader.ReadInt16();
            var savedObjects = new List<ObjectData>();
            foreach (var __ in Enumerable.Range(0, numSavedObjects))
                savedObjects.Add(ObjectData.LoadFrom(reader));
            m_savedSceneData[savedSceneName] = savedObjects;
        }

        // Don't save current scene (there might be one) to avoid mixing old and new data.
        yield return LoadNewSceneWithoutSavingCurrentAsync(newScene);
    }

    public IEnumerator StartFresh(string sceneName)
    {
        yield return LoadNewSceneWithoutSavingCurrentAsync(sceneName);
    }

    IEnumerator LoadNewSceneWithoutSavingCurrentAsync(string sceneName)
    {
        if (m_currentScene != null)
        {
            // TODO: Do this concurrently with loading the new scene?
            // (watch out for concurrent modifications to data)
            yield return CloseCurrentSceneWithoutSavingAsync();
        }

        Debug.Log($"Loading {sceneName}");
        m_currentScene = sceneName;
        m_currentSceneData = new HashSet<PersistablePrefab>();
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Debug.Log($"Done loading {sceneName}");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));

        // If we've already initialized this scene, then destroy any persistable objects loaded by
        // it (since they are used for initialization).
        if (m_initializedScenes.Contains(sceneName))
        {
            var currentSceneData = new HashSet<PersistablePrefab>(m_currentSceneData);
            m_currentSceneData.Clear();

            foreach (var saveableObject in currentSceneData)
            {
                Object.Destroy(saveableObject.gameObject);
            }

            Debug.Assert(m_currentSceneData.Count == 0,
                "Expected no new PersistablePrefab instances to be added while destroying objects");
        }

        // Load any data saved for the scene already.
        if (m_savedSceneData.TryGetValue(sceneName, out var savedData))
        {
            foreach (var savedObject in savedData)
            {
                savedObject.Instantiate();
            }
        }
    }

    IEnumerator CloseCurrentSceneWithoutSavingAsync()
    {
        Debug.Assert(m_currentScene != null);
        Debug.Assert(m_currentSceneData != null);

        Debug.Log($"Unloading {m_currentScene}");
        yield return SceneManager.UnloadSceneAsync(m_currentScene);
        m_currentScene = null;
        m_currentSceneData = null;
        Debug.Log($"Done unloading {m_currentScene}");
    }

    void SaveCurrentScene()
    {
        if (m_currentScene == null)
            return;

        m_initializedScenes.Add(m_currentScene);
        m_savedSceneData[m_currentScene] = new List<ObjectData>();
        foreach (var obj in m_currentSceneData)
        {
            AppendToScene(m_currentScene, obj);
        }
    }

    void AppendToScene(string sceneName, PersistablePrefab rootObject)
    {
        if (rootObject.HasParent)
        {
            Debug.LogError("Tried to move non-root object to a new scene, which is not supported",
                rootObject.gameObject);
            return;
        }

        if (!m_savedSceneData.TryGetValue(sceneName, out var sceneData))
        {
            m_savedSceneData[sceneName] = sceneData = new List<ObjectData>();
        }

        sceneData.Add(ObjectData.Serialize(rootObject));
    }

    HashSet<PersistablePrefab> m_currentSceneData;
    string m_currentScene;

    readonly Dictionary<string, List<ObjectData>> m_savedSceneData =
        new Dictionary<string, List<ObjectData>>();

    /// <summary>
    /// Scenes that have been initialized.
    /// 
    /// Every scene in this set has an entry in <see cref="m_savedSceneData"/>.
    /// </summary>
    readonly HashSet<string> m_initializedScenes = new HashSet<string>();
}
