using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singletons that are available at all times when the application is running.
/// </summary>
public sealed class MainSingletons : MonoBehaviour
{
    #region Singleton
    public static MainSingletons Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
            Debug.LogError($"MainSingletons instance already exists: {Instance}", gameObject);
        else
            Instance = this;
    }
    #endregion

    public void StartNewGame()
    {
        Debug.Log("Starting new game");

        if (m_isInGame)
        {
            Debug.LogError("Tried to start a new game while still in a game."
                + " This is not supported.");
            return;
        }

        m_isInGame = true;

        StartCoroutine(StartNewGameAsync());
    }

    IEnumerator StartNewGameAsync()
    {
        UseLoadingCamera();
        yield return LoadGameSingletonsSceneAsync();
        yield return GameSingletons.Instance.StartNewGameAsync();
        yield return new WaitForSeconds(1);
        UseGameCamera();
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void UseLoadingCamera()
    {
        mainCamera.gameObject.SetActive(false);
        loadingCamera.gameObject.SetActive(true);
    }

    void UseGameCamera()
    {
        mainCamera.gameObject.SetActive(false);
        loadingCamera.gameObject.SetActive(false);
        GameSingletons.Instance.ActivateGameCamera();
    }

    IEnumerator LoadGameSingletonsSceneAsync()
    {
        yield return SceneManager.LoadSceneAsync("GameSingletonsScene", LoadSceneMode.Additive);
    }

    bool m_isInGame = false;

    [SerializeField] Camera loadingCamera;
    [SerializeField] Camera mainCamera;
}
