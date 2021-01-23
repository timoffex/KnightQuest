using UnityEngine;

/// <summary>
/// An object that is ignored by the <see cref="PersistablePrefab"/> hierarchy.
/// 
/// This is intended for organizational purposes. <see cref="PersistablePrefab"/> instances will
/// skip objects with this component when looking for their <see cref="PersistablePrefab"/> parent.
/// </summary>
public sealed class IgnoreForPersistence : MonoBehaviour
{
#if UNITY_EDITOR
    void OnValidate()
    {
        if (GetComponent<PersistablePrefab>() != null)
        {
            Debug.LogWarning(
                $"{gameObject} has both IgnoreForPersistence and PersistablePrefab, which is not"
                + " valid", gameObject);
        }
    }
#endif
}
