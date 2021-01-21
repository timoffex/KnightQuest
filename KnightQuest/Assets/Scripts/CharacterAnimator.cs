using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public sealed class CharacterAnimator : MonoBehaviour
{
    Animator m_animator;
    SpriteRenderer m_spriteRenderer;
    Rigidbody2D m_rigidbody2D;
    Character m_character;

    Color m_defaultTint = Color.white;
    bool m_isSpriteWhite = false;
    Shader m_whiteShader;
    Shader m_defaultShader;

    public void FlashWhite()
    {
        StartCoroutine(FlashWhiteCR());
    }

    public void TintRedForFire()
    {
        m_defaultTint = Color.red;
        if (!m_isSpriteWhite)
            m_spriteRenderer.color = m_defaultTint;
    }

    IEnumerator FlashWhiteCR()
    {
        // Don't flash while already flashing.
        if (m_isSpriteWhite)
            yield break;

        UseWhiteSprite();
        yield return new WaitForSeconds(0.1f);
        UseNormalSprite();
    }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_character = GetComponentInParent<Character>();
        m_rigidbody2D = GetComponentInParent<Rigidbody2D>();

        // https://answers.unity.com/questions/582145/is-there-a-way-to-set-a-sprites-color-solid-white.html
        m_whiteShader = Shader.Find("GUI/Text Shader");
        m_defaultShader = m_spriteRenderer.material.shader;

        GetComponentInParent<CharacterAnimationController>().RegisterCharacterAnimator(this);
    }

    void LateUpdate()
    {
        m_animator.SetFloat(
            "speedPercentage",
            Mathf.Clamp01(m_rigidbody2D.velocity.magnitude / m_character.MaxSpeed));
        m_animator.SetInteger("direction", m_character.Direction.ToInteger());
    }

    void UseWhiteSprite()
    {
        if (m_isSpriteWhite)
            return;

        m_spriteRenderer.material.shader = m_whiteShader;
        m_spriteRenderer.color = Color.white;
        m_isSpriteWhite = true;
    }

    void UseNormalSprite()
    {
        if (!m_isSpriteWhite)
            return;

        m_spriteRenderer.material.shader = m_defaultShader;
        m_spriteRenderer.color = m_defaultTint;
        m_isSpriteWhite = false;
    }
}
