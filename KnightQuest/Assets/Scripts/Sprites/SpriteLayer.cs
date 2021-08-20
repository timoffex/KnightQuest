using UnityEngine;

/// <summary>
/// A component that displays a given <see cref="AnimatedSprite"/>.
///
/// This should be created programmatically with <see cref="Create"/> and destroyed with
/// the <see cref="Destroy"/> method on this class.
/// </summary>
public sealed class SpriteLayer : MonoBehaviour
{
    AnimatedSprite m_animatedSprite;

    SpriteRenderer m_backSpriteRenderer;
    SpriteRenderer m_frontSpriteRenderer;
    Shader m_defaultShader;

    // https://answers.unity.com/questions/582145/is-there-a-way-to-set-a-sprites-color-solid-white.html
    static Shader m_whiteShader => m_whiteShaderCached != null
        ? m_whiteShaderCached
        : m_whiteShaderCached = Shader.Find("GUI/Text Shader");
    static Shader m_whiteShaderCached;

    void Initialize()
    {
        m_backSpriteRenderer =
            GameObjectUtility
                .CreateChild("Back", transform)
                .AddComponent<SpriteRenderer>();
        m_backSpriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;

        m_frontSpriteRenderer =
            GameObjectUtility
                .CreateChild("Front", transform)
                .AddComponent<SpriteRenderer>();
        m_frontSpriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;

        m_defaultShader = m_frontSpriteRenderer.sharedMaterial.shader;
    }

    public static SpriteLayer Create(string name, Transform parent, int layerIndex)
    {
        var layer = GameObjectUtility.CreateChild(name, parent).AddComponent<SpriteLayer>();
        layer.Initialize();
        layer.SetLayerIndex(layerIndex);
        return layer;
    }

    public void Destroy() => GameObjectUtility.Destroy(gameObject);

    public void SetAnimatedSprite(AnimatedSprite animatedSprite)
    {
        m_animatedSprite = animatedSprite;
    }

    /// <summary>
    /// Sets the layer's index, which is used to adjust the back and front renderer positions so
    /// that they display in the correct order in relation to other layers.
    /// </summary>
    public void SetLayerIndex(int index)
    {
        Debug.Assert(index >= 0);
        m_backSpriteRenderer.transform.localPosition =
                new Vector3(0, 0, index * 0.0001f);
        m_frontSpriteRenderer.transform.localPosition =
                new Vector3(0, 0, -index * 0.0001f);
    }

    public void ShowAnimationFrame(string animation, int frame, CharacterDirection direction)
    {
        Debug.Assert(m_animatedSprite != null);

        m_animatedSprite.SetSprite(
            animation,
            frame,
            direction,
            backRenderer: m_backSpriteRenderer,
            frontRenderer: m_frontSpriteRenderer);
    }

    /// <summary>
    /// Sets a tint on the layer's sprite renderers.
    /// </summary>
    public void MakeTinted(Color color)
    {
        ResetShader();
        SetTint(color);
    }

    public void MakePureColor(Color color)
    {
        SetShader(m_whiteShader);
        SetTint(color);
    }

    void SetTint(Color color)
    {
        m_backSpriteRenderer.color = color;
        m_frontSpriteRenderer.color = color;
    }

    void SetShader(Shader shader)
    {
        m_backSpriteRenderer.material.shader = shader;
        m_frontSpriteRenderer.material.shader = shader;
    }

    void ResetShader()
    {
        SetShader(m_defaultShader);
    }
}