using UnityEngine;

public sealed class GameObjectUtility
{
    /// <summary>
    /// Creates a centered child GameObject of the given transform.
    /// </summary>
    /// <remarks>
    /// By default, this sets the <see cref="HideFlags.HideAndDontSave"/> hideflags, which has no
    /// effect at runtime but is useful for scripts that run in the editor.
    /// </remarks>
    public static GameObject CreateChild(
        string name, Transform parent, HideFlags hideFlags = HideFlags.HideAndDontSave)
    {
        var go = new GameObject(name);
        go.hideFlags = hideFlags;
        go.transform.parent = parent;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        return go;
    }

    /// <summary>
    /// Destroys the object by using Destroy() if in play mode, or using DestroyImmediate() in a
    /// delayed call if in editor.
    /// </summary>
    /// <remarks>
    /// In the editor, you must call DestroyImmediate() instead of Destroy(). However, neither of
    /// those is allowed in OnValidate() or in Awake(), so the call must be delayed.
    /// </remarks>
    public static void Destroy(Object obj)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.delayCall += () => Object.DestroyImmediate(obj);
            return;
        }
#endif
        Destroy(obj);
    }
}