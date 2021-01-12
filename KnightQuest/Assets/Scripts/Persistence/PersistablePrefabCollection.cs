using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "Game/Persistable Prefab Collection",
    fileName = "All Persistable Prefabs")]
public sealed class PersistablePrefabCollection
    : ScriptableObject,
      ISerializationCallbackReceiver
{
    [SerializeField] Element[] elements;

    Dictionary<PersistablePrefab, string> m_prefabToId;
    Dictionary<string, PersistablePrefab> m_idToPrefab;

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        m_prefabToId = new Dictionary<PersistablePrefab, string>();
        m_idToPrefab = new Dictionary<string, PersistablePrefab>();

        foreach (var element in elements)
        {
            if (m_prefabToId.ContainsKey(element.prefab) ||
                m_idToPrefab.ContainsKey(element.id))
            {
                continue;
            }

            m_prefabToId.Add(element.prefab, element.id);
            m_idToPrefab.Add(element.id, element.prefab);
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
    }

    public string GetPrefabId(PersistablePrefab prefab)
    {
        if (!m_prefabToId.TryGetValue(prefab, out var id))
        {
            Debug.LogError($"Object {prefab} is not a registered prefab", this);
            return null;
        }

        return id;
    }

    public PersistablePrefab GetPrefabById(string id)
    {
        if (!m_idToPrefab.TryGetValue(id, out var prefab))
        {
            Debug.LogError($"ID {id} does not have an associated prefab", this);
            return null;
        }

        return prefab;
    }

    [ContextMenu("Validate Entries")]
    public void ValidateEntries()
    {
        var seenIds = new Dictionary<string, PersistablePrefab>();
        var seenPrefabs = new Dictionary<PersistablePrefab, string>();

        foreach (var entry in elements)
        {
            if (entry.prefab.SceneId != null)
            {
                Debug.LogError(
                    $"Prefab has a scene ID but shouldn't: {entry.prefab}", entry.prefab);
            }

            if (entry.prefab.PrefabId != entry.id)
            {
                Debug.LogError(
                    $"Prefab {entry.prefab} has a different ID configured than what is in the"
                    + " collection", entry.prefab);
            }

            if (seenIds.TryGetValue(entry.id, out var idPrefab))
            {
                Debug.LogError(
                    $"ID {entry.id} is used for more than one prefab: {idPrefab}, {entry.prefab}");
            }
            else
            {
                seenIds[entry.id] = entry.prefab;
            }

            if (seenPrefabs.TryGetValue(entry.prefab, out var prefabId))
            {
                Debug.LogError(
                    $"Prefab {entry.prefab} is registered more than once");
            }
            else
            {
                seenPrefabs[entry.prefab] = entry.id;
            }
        }
    }

    [System.Serializable]
    public class Element
    {
        public PersistablePrefab prefab;
        public string id;
    }
}
