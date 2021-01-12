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

    Dictionary<GameObject, string> m_prefabToId;
    Dictionary<string, GameObject> m_idToPrefab;

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        m_prefabToId = new Dictionary<GameObject, string>();
        m_idToPrefab = new Dictionary<string, GameObject>();

        foreach (var element in elements)
        {
            if (m_prefabToId.TryGetValue(element.prefab, out var prefabId))
            {
                Debug.LogError($"Prefab is already registered for ID '{prefabId}'");
            }
            else if (m_idToPrefab.TryGetValue(element.id, out var idPrefab))
            {
                Debug.LogError(
                    $"Id '{element.id}' has more than one registered prefab", idPrefab);
            }
            else
            {
                m_prefabToId.Add(element.prefab, element.id);
                m_idToPrefab.Add(element.id, element.prefab);
            }
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
    }

    public string GetPrefabId(GameObject prefab)
    {
        if (!m_prefabToId.TryGetValue(prefab, out var id))
        {
            Debug.LogError($"Object {prefab} is not a registered prefab", this);
            return null;
        }

        return id;
    }

    public GameObject GetPrefabById(string id)
    {
        if (!m_idToPrefab.TryGetValue(id, out var prefab))
        {
            Debug.LogError($"ID {id} does not have an associated prefab", this);
            return null;
        }

        return prefab;
    }

    [System.Serializable]
    public class Element
    {
        public GameObject prefab;
        public string id;
    }
}
