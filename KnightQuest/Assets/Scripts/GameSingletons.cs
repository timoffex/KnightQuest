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
            Debug.LogError($"GameSingletons instance already exists: {Instance}", gameObject);
        else
            Instance = this;

        GameData = new GameData();
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    [SerializeField] Camera mainCamera;

    [SerializeField] PersistablePrefabCollection prefabCollection;

    [SerializeField] FireSystem fireSystem;

    public PersistablePrefabCollection PrefabCollection => prefabCollection;

    public FireSystem FireSystem => fireSystem;

    public Vector2 MouseWorldPosition =>
        mainCamera.ScreenToWorldPoint(Input.mousePosition);

    /// <summary>
    /// The <see cref="Character"/> the player is currently playing. May be null.
    /// </summary>
    public Character PlayerCharacter { get; set; }

    /// <summary>
    /// Information about the current player. May be null.
    /// </summary>
    public Player Player { get; set; }

    public GameData GameData { get; private set; }

    public IEnumerator StartNewGameAsync()
    {
        return GameData.StartFresh("SampleScene");
    }

    public void MovePlayerToAndStartScene(string sceneName, Vector3 position)
    {
        StartCoroutine(MovePlayerToAndStartSceneAsync(sceneName, position));
    }

    IEnumerator MovePlayerToAndStartSceneAsync(string sceneName, Vector3 position)
    {
        if (PlayerCharacter != null)
        {
            PlayerCharacter.transform.position = position;
            GameData.MoveToOtherScene(
                PlayerCharacter.GetComponent<PersistablePrefab>(),
                sceneName);
        }

        MainSingletons.Instance.UseLoadingCamera();
        yield return GameData.LoadSceneAsync(sceneName);
        MainSingletons.Instance.UseGameCamera();
    }

    public void SaveTo(GameDataWriter writer)
    {
        GameData.SaveTo(writer);
    }

    /// <summary>
    /// Loads the game from the given <paramref name="reader"/> and opens its game scenes.
    /// </summary>
    public IEnumerator LoadFromAsync(GameDataReader reader)
    {
        MainSingletons.Instance.UseLoadingCamera();
        yield return GameData.LoadFromAndStartAsync(reader);
        MainSingletons.Instance.UseGameCamera();
    }

    public void ActivateGameCamera()
    {
        foreach (var virtualCamera in m_sceneCameras)
        {
            virtualCamera.Enable();
        }

        mainCamera.gameObject.SetActive(true);
    }

    public void DeactivateGameCamera()
    {
        foreach (var virtualCamera in m_sceneCameras)
        {
            virtualCamera.Disable();
        }

        mainCamera.gameObject.SetActive(false);
    }

    public void AddGameSceneCamera(GameSceneVirtualCamera virtualCamera)
    {
        m_sceneCameras.Add(virtualCamera);
    }

    public void RemoveGameSceneCamera(GameSceneVirtualCamera virtualCamera)
    {
        m_sceneCameras.Remove(virtualCamera);
    }

    readonly HashSet<GameSceneVirtualCamera> m_sceneCameras = new HashSet<GameSceneVirtualCamera>();
}
