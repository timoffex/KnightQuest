using UnityEngine;

/// <summary>
/// A component that displays a given <see cref="AnimatedSprite"/>.
/// </summary>
public sealed class SpriteLayer : MonoBehaviour
{
    AnimatedSprite m_animatedSprite;

    SpriteRenderer m_backSpriteRenderer;
    SpriteRenderer m_frontSpriteRenderer;

    void Initialize()
    {
        var backGo = new GameObject("Back");
        backGo.transform.SetParent(transform);
        m_backSpriteRenderer = backGo.AddComponent<SpriteRenderer>();

        var frontGo = new GameObject("Front");
        frontGo.transform.SetParent(transform);
        m_frontSpriteRenderer = frontGo.AddComponent<SpriteRenderer>();
    }

    public static SpriteLayer Create(string name, Transform parent, int layerIndex)
    {
        var layerGo = new GameObject(name);
        layerGo.transform.SetParent(parent);
        var layer = layerGo.AddComponent<SpriteLayer>();
        layer.Initialize();
        layer.SetLayerIndex(layerIndex);
        return layer;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetAnimatedSprite(AnimatedSprite animatedSprite)
    {
        m_animatedSprite = animatedSprite;
    }

    public void SetLayerIndex(int index)
    {
        Debug.Assert(index >= 0);
        m_backSpriteRenderer.transform.position = new Vector3(0, 0, index * 0.0001f);
        m_frontSpriteRenderer.transform.position = new Vector3(0, 0, -index * 0.0001f);
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
}