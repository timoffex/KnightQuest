using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSingletons : MonoBehaviour
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
    #endregion

    [SerializeField] Camera mainCamera;

    public Vector2 MouseWorldPosition =>
        mainCamera.ScreenToWorldPoint(Input.mousePosition);
}
