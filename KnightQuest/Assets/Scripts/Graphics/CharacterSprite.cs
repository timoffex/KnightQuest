using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A component that displays the layers of a customizable character.
/// </summary>
public sealed class CharacterSprite : MonoBehaviour
{
    SpriteLayer m_hair;
    SpriteLayer m_outline;
    SpriteLayer m_skin;

    IEnumerable<SpriteLayer> AllLayers
    {
        get
        {
            if (m_hair != null) yield return m_hair;
            if (m_outline != null) yield return m_outline;
            if (m_skin != null) yield return m_skin;
        }
    }

    public void SetHairSprite(AnimatedSprite sprite)
    {
        if (sprite == null && m_hair != null)
        {
            m_hair.Destroy();
        }
        else
        {
            if (m_hair == null) m_hair = NewSpriteLayer("Hair", 1);
            m_hair.SetAnimatedSprite(sprite);
        }
    }

    public void SetOutlineSprite(AnimatedSprite sprite)
    {
        if (sprite == null && m_outline != null)
        {
            m_outline.Destroy();
        }
        else
        {
            if (m_outline == null) m_outline = NewSpriteLayer("Outline", 2);
            m_outline.SetAnimatedSprite(sprite);
        }
    }

    public void SetSkinSprite(AnimatedSprite sprite)
    {
        if (sprite == null && m_skin != null)
        {
            m_skin.Destroy();
        }
        else
        {
            if (m_skin == null) m_skin = NewSpriteLayer("Skin", 0);
            m_skin.SetAnimatedSprite(sprite);
        }
    }

    public void ShowAnimationFrame(string animation, int frame, CharacterDirection direction)
    {
        foreach (var layer in AllLayers)
        {
            layer.ShowAnimationFrame(animation, frame, direction);
        }
    }

    SpriteLayer NewSpriteLayer(string name, int layerIndex)
    {
        return SpriteLayer.Create(name, transform, layerIndex);
    }

    // Used by Preview(); should not be set otherwise.
    // Apparently it's a bad idea to guard serialized fields by UNITY_EDITOR
    [SerializeField] AnimatedSprite m_previewHair;
    [SerializeField] AnimatedSprite m_previewOutline;
    [SerializeField] AnimatedSprite m_previewSkin;
    [SerializeField] string m_previewAnimation;

#if UNITY_EDITOR
    [ContextMenu("Preview Character")]
    public void Preview()
    {
        SetHairSprite(m_previewHair);
        SetOutlineSprite(m_previewOutline);
        SetSkinSprite(m_previewSkin);
        ShowAnimationFrame(m_previewAnimation, 0, CharacterDirection.Down);
    }
#endif
}
