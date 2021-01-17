using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public sealed class PlayerCharacterController : PersistableComponent
{
    Character m_character;

    Weapon m_attackingWeapon;
    GameSingletons m_gameSingletons;

    protected override void Awake()
    {
        base.Awake();
        m_character = GetComponent<Character>();
    }

    void Start()
    {
        m_gameSingletons = GameSingletons.Instance;
        m_gameSingletons.PlayerCharacter = m_character;
    }

    void Update()
    {
        Move();

        m_character.CurrentWeapon?.AlignToward(m_gameSingletons.MouseWorldPosition);

        if (m_attackingWeapon != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_attackingWeapon.OnAttackButtonUp();
                m_attackingWeapon = null;
            }
        }
        else if (m_character.CurrentWeapon != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_attackingWeapon = m_character.CurrentWeapon;
                m_attackingWeapon.OnAttackButtonDown();
            }
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (x != 0 || y != 0)
            m_character.MoveInDirection(new Vector2(x, y).normalized);
    }
}
