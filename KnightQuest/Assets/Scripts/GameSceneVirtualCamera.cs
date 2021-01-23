using UnityEngine;
using Cinemachine;

/// <summary>
/// A component to add to every Cinemachine virtual camera object in a game scene.
/// 
/// This disables the virtual camera on Awake() and allows it to be re-enabled once the scene
/// finishes loading.
/// </summary>
[RequireComponent(typeof(CinemachineVirtualCamera))]
public sealed class GameSceneVirtualCamera : MonoBehaviour
{
    public void Enable()
    {
        m_camera.enabled = true;
    }

    public void Disable()
    {
        m_camera.enabled = false;
    }

    void Awake()
    {
        m_camera = GetComponent<CinemachineVirtualCamera>();
        m_camera.enabled = false;

        GameSingletons.Instance.AddGameSceneCamera(this);
    }

    void OnDestroy()
    {
        // Can be null if the game is being unloaded
        GameSingletons.Instance?.RemoveGameSceneCamera(this);
    }
    CinemachineVirtualCamera m_camera;
}
