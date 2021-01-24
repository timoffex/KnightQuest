using UnityEngine;

/// <summary>
/// A trigger zone that moves the player to a new region.
/// </summary>
public sealed class GoToNewSceneTrigger : PlayerTriggerZone
{
    [SerializeField] string sceneName;

    GameSingletons m_gameSingletons;

    protected override void Start()
    {
        base.Start();
        m_gameSingletons = GameSingletons.Instance;
    }

    protected override void OnPlayerEntered(Player player)
    {
        m_gameSingletons.MovePlayerToAndStartScene(sceneName, Vector3.zero);
    }
}
