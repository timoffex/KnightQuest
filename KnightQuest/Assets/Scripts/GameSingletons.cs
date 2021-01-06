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

    public Vector2 MouseWorldPosition =>
        mainCamera.ScreenToWorldPoint(Input.mousePosition);
}
