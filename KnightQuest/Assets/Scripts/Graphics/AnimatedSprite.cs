using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of all sprite animations supported by some 2D object.
///
/// For example, this could be all frames for all directions of a "Swing1" and "Swing2" animation
/// on a Sword.
/// </summary>
[CreateAssetMenu(menuName = "New AnimatedSprite", fileName = "NewAnimatedSprite")]
public sealed class AnimatedSprite : ScriptableObject
{
    [SerializeField] SpriteAnimation[] m_animations;

    /// <summary>
    /// Configures the back and front SpriteRenderers to display the frame of the animation of
    /// this animated sprite.
    /// 
    /// The named animation must exist on this sprite; see <see cref="HasAnimation(string)"/>.
    /// 
    /// If there is no front or back frame corresponding to the index (assuming the index is valid)
    /// then this disables the corresponding renderer.
    /// </summary>
    public void SetSprite(
            string animation,
            int frame,
            CharacterDirection direction,
            SpriteRenderer backRenderer,
            SpriteRenderer frontRenderer)
    {
        Debug.Assert(HasAnimation(animation), $"Animation {animation} doesn't exist on {this}");
        AnimationByName(animation).SetSprite(
            frame,
            direction,
            backRenderer: backRenderer,
            frontRenderer: frontRenderer);
    }

    /// <summary>
    /// Returns whether this sprite supports the specified animation.
    /// </summary>
    public bool HasAnimation(string animation)
    {
        return m_animations.Where(x => x.Name == animation).Any();
    }

    /// <summary>
    /// Enumerates the names of all animations supported by this sprite.
    /// </summary>
    public IEnumerable<string> SupportedAnimations =>
        m_animations.Select(animation => animation.Name);

    SpriteAnimation AnimationByName(string animation)
    {
        return m_animations.First(x => x.Name == animation);
    }
}
