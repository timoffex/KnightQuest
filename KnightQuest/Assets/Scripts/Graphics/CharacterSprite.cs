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

    Color m_hairTint = Color.white;
    Color m_skinTint = Color.white;

    IEnumerable<SpriteLayer> AllLayers
    {
        get
        {
            if (m_hair != null) yield return m_hair;
            if (m_outline != null) yield return m_outline;
            if (m_skin != null) yield return m_skin;
        }
    }

    public static CharacterSprite Create(
            string name,
            Transform parent)
    {
        var go = new GameObject(name);
        go.transform.parent = parent;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        return go.AddComponent<CharacterSprite>();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void SetHairTint(Color tint)
    {
        m_hairTint = tint;
    }

    public void SetSkinTint(Color tint)
    {
        m_skinTint = tint;
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

    /// <summary>
    /// Tints all layers of this sprite.
    /// </summary>
    public void MakeTinted(Color color)
    {
        if (m_hair != null) m_hair.MakeTinted(color * m_hairTint);
        if (m_skin != null) m_skin.MakeTinted(color * m_skinTint);
        if (m_outline != null) m_outline.MakeTinted(color);
    }

    /// <summary>
    /// Turns this sprite a single color (in the shape of the sprite).
    /// </summary>
    public void MakePureColor(Color color)
    {
        foreach (var layer in AllLayers)
        {
            layer.MakePureColor(color);
        }
    }

    SpriteLayer NewSpriteLayer(string name, int layerIndex)
    {
        return SpriteLayer.Create(name, transform, layerIndex);
    }
}
