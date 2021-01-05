using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAttack : MonoBehaviour
{
    [SerializeField] ArrowSpawner arrowSpawner;

    [SerializeField] float arrowSpeed;

    [SerializeField] Transform rotationPoint;

    GameSingletons m_gameSingletons;
    Quaternion m_initialRotation;
    Vector3 m_initialOffset;

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
        m_initialRotation = transform.rotation;
        m_initialOffset = transform.position - rotationPoint.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Rotate based on click direction.
            var clickPosition = m_gameSingletons.MouseWorldPosition;
            var attackDirection = clickPosition - (Vector2)rotationPoint.position;
            float angle = Vector2.SignedAngle(Vector2.right, attackDirection);
            transform.rotation = m_initialRotation;
            transform.position = rotationPoint.position + m_initialOffset;
            transform.RotateAround(rotationPoint.position, Vector3.forward, angle);

            // Spawn an arrow.
            arrowSpawner.Spawn(transform.position, attackDirection.normalized * arrowSpeed);
        }
    }

    void OnDrawGizmos()
    {
        if (rotationPoint == null)
            return;

        float radius = Vector3.Distance(rotationPoint.position, transform.position);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(rotationPoint.position, radius);
    }
}
