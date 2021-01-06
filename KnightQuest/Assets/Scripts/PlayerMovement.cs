using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Character))]
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Multiplier for axis values to get desired speed.")]
    public float controlsForceMultiplier = 1;

    Rigidbody2D m_rigidbody2D;
    Character m_character;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        m_rigidbody2D.AddForce(new Vector2(x, y) * controlsForceMultiplier, ForceMode2D.Force);
        if (m_rigidbody2D.velocity.magnitude > m_character.MaxSpeed)
        {
            m_rigidbody2D.velocity = m_rigidbody2D.velocity.normalized * m_character.MaxSpeed;
        }
    }
}
