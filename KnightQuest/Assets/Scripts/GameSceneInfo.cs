using UnityEngine;

/// <summary>
/// Information associated to an entire game scene, such as its background color.
/// </summary>
/// <remarks>
/// To keep track of marked "objects" in the scene, I should probably follow the
/// <see cref="GameSceneVirtualCamera"/> pattern instead (that is add a component to those objects
/// instead of dragging their references here).
/// </remarks>
public sealed class GameSceneInfo : MonoBehaviour
{
    [SerializeField] Color backgroundColor;

    void Awake()
    {
        GameSingletons.Instance.SetGameSceneBackgroundColor(backgroundColor);
    }
}
