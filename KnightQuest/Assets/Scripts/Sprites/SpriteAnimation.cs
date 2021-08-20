using System.Collections.Generic;
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

    public static int FrameCountInAnimation(string animation)
    {
        Debug.Assert(animation.Length > 0, "Got empty animation name");
        if (!m_animationFrameCount.TryGetValue(animation, out var frameCount))
        {
            Debug.LogError($"No known '{animation}' animation");
            return 0;
        }

        return frameCount;
    }

#if UNITY_EDITOR
    public static SpriteAnimation CreateInstance(
        string name,
        Sprite[] frontUpFrames,
        Sprite[] frontDownFrames,
        Sprite[] frontLeftFrames,
        Sprite[] frontRightFrames,
        Sprite[] backUpFrames,
        Sprite[] backDownFrames,
        Sprite[] backLeftFrames,
        Sprite[] backRightFrames
    )
    {
        var spriteAnimation = CreateInstance<SpriteAnimation>();

        spriteAnimation.m_animationName = name;
        spriteAnimation.m_frontUpFrames = frontUpFrames;
        spriteAnimation.m_frontUpFrames = frontUpFrames;
        spriteAnimation.m_frontDownFrames = frontDownFrames;
        spriteAnimation.m_frontLeftFrames = frontLeftFrames;
        spriteAnimation.m_frontRightFrames = frontRightFrames;
        spriteAnimation.m_backUpFrames = backUpFrames;
        spriteAnimation.m_backDownFrames = backDownFrames;
        spriteAnimation.m_backLeftFrames = backLeftFrames;
        spriteAnimation.m_backRightFrames = backRightFrames;

        spriteAnimation.Awake();

        return spriteAnimation;
    }
#endif

    /// <summary>
    /// The number of frames in the animation.
    /// </summary>
    public int FrameCount => CachedFrameCount;
    int CachedFrameCount;

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

    void Awake()
    {
        // This happens in CreateInstance()
        if (Name == null) return;

        CachedFrameCount = CalculateFrameCount();
        if (m_animationFrameCount.TryGetValue(Name, out var existingFrameCount))
        {
            if (existingFrameCount != CachedFrameCount)
            {
                Debug.LogError(
                    $"Inconsistent frame counts for animation {Name}: " +
                    $"{existingFrameCount} vs {CachedFrameCount}. Using the smaller one.");
                m_animationFrameCount[Name] = Mathf.Min(existingFrameCount, CachedFrameCount);
            }
        }
        else
        {
            m_animationFrameCount[Name] = CachedFrameCount;
        }
    }

    void OnValidate()
    {
        CachedFrameCount = CalculateFrameCount();
        m_animationFrameCount[Name] = CachedFrameCount;
    }

    int CalculateFrameCount()
    {
        // All directions must have the same number of frames, but some directions might be
        // unset
        if (m_frontDownFrames.Length > 0) return m_frontDownFrames.Length;
        if (m_frontUpFrames.Length > 0) return m_frontUpFrames.Length;
        if (m_frontLeftFrames.Length > 0) return m_frontLeftFrames.Length;
        if (m_frontRightFrames.Length > 0) return m_frontRightFrames.Length;

        if (m_backDownFrames.Length > 0) return m_backDownFrames.Length;
        if (m_backUpFrames.Length > 0) return m_backUpFrames.Length;
        if (m_backLeftFrames.Length > 0) return m_backLeftFrames.Length;
        if (m_backRightFrames.Length > 0) return m_backRightFrames.Length;

        return 0;
    }

    readonly static Dictionary<string, int> m_animationFrameCount = new Dictionary<string, int>();
}
