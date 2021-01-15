using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public sealed class CameraFollowPlayer : MonoBehaviour
{
    GameSingletons m_gameSingletons;
    CinemachineVirtualCamera m_camera;

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
        m_camera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (m_gameSingletons.PlayerCharacter != null)
            m_camera.Follow = m_gameSingletons.PlayerCharacter.transform;
    }
}
