using UnityEngine;

/// <summary>
/// A collection of sprites corresponding to frames and directions of a motion.
/// 
/// For example, this could be all frames for all directions of a Walk animation for a Cow sprite.
/// </summary>
[CreateAssetMenu(menuName = "New SpriteAnimation", fileName = "NewSpriteAnimation")]
public sealed class SpriteAnimation : ScriptableObject
{
    public string Name => m_animationName;
    [SerializeField] string m_animationName;

    [SerializeField] Sprite[] m_frontUpFrames;
    [SerializeField] Sprite[] m_frontDownFrames;
    [SerializeField] Sprite[] m_frontLeftFrames;
    [SerializeField] Sprite[] m_frontRightFrames;

    [SerializeField] Sprite[] m_backUpFrames;
    [SerializeField] Sprite[] m_backDownFrames;
    [SerializeField] Sprite[] m_backLeftFrames;
    [SerializeField] Sprite[] m_backRightFrames;

    /// <summary>
    /// Configures the back and front SpriteRenderers to display a frame of this animation.
    /// 
    /// If there is no front or back frame corresponding to the index (assuming the index is valid)
    /// then this disables the corresponding renderer.
    /// </summary>
    public void SetSprite(
            int frame,
            CharacterDirection direction,
            SpriteRenderer backRenderer,
            SpriteRenderer frontRenderer)
    {
        var frontFrames = FrontFramesFor(direction);
        var flipFront = false;
        if (frontFrames.Length == 0)
        {
            frontFrames = FrontFramesFor(direction.FlipHorizontally());
            flipFront = true;
        }

        var backFrames = BackFramesFor(direction);
        var flipBack = false;
        if (backFrames.Length == 0)
        {
            backFrames = BackFramesFor(direction.FlipHorizontally());
            flipBack = true;
        }

        if (frontFrames.Length > 0)
        {
            frontRenderer.sprite = frontFrames[frame];
            frontRenderer.enabled = true;
            frontRenderer.flipX = flipFront;
        }
        else
        {
            frontRenderer.enabled = false;
        }

        if (backFrames.Length > 0)
        {
            backRenderer.sprite = backFrames[frame];
            backRenderer.enabled = true;
            backRenderer.flipX = flipBack;
        }
        else
        {
            backRenderer.enabled = false;
        }
    }

    Sprite[] FrontFramesFor(CharacterDirection direction)
    {
        switch (direction)
        {
            case CharacterDirection.Up:
            case CharacterDirection.LeftUp:
            case CharacterDirection.RightUp:
                return m_frontUpFrames;
            case CharacterDirection.Down:
            case CharacterDirection.LeftDown:
            case CharacterDirection.RightDown:
                return m_frontDownFrames;
            case CharacterDirection.Left:
                return m_frontLeftFrames;
            case CharacterDirection.Right:
                return m_frontRightFrames;
            default:
                return new Sprite[0];
        }
    }

    Sprite[] BackFramesFor(CharacterDirection direction)
    {
        switch (direction)
        {
            case CharacterDirection.Up:
            case CharacterDirection.LeftUp:
            case CharacterDirection.RightUp:
                return m_backUpFrames;
            case CharacterDirection.Down:
            case CharacterDirection.LeftDown:
            case CharacterDirection.RightDown:
                return m_backDownFrames;
            case CharacterDirection.Left:
                return m_backLeftFrames;
            case CharacterDirection.Right:
                return m_backRightFrames;
            default:
                return new Sprite[0];
        }
    }
}
