using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public sealed class PlayerMovement : MonoBehaviour
{
    Character m_character;

    // Start is called before the first frame update
    void Start()
    {
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
        m_character.MoveInDirection(new Vector2(x, y));
    }
}
