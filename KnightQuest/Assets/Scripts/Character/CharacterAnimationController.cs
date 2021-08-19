using System.Collections;
using UnityEngine;

/// <summary>
/// Component that goes on a <see cref="Character"/> object that helps coordinate complex character
/// animations.
/// </summary>
/// <remarks>
/// I considered putting all of this code into <see cref="Character"/>, but that makes that class
/// a lot more difficult to read and use. From outside of the <see cref="Character"/> object, this
/// component should be viewed as an implementation detail.
/// 
/// As usual, I'm not yet sure whether this is a good idea, but I'm not going to dwell on it 
/// because it will be easy to refactor. For now, it makes code cleaner.
/// 
/// Also note that this whole thing can be designed in other ways, such as using polling or events.
/// <see cref="CharacterAnimator"/> actually polls <see cref="Character"/> (unless I changed that)
/// for direction and speed data, but it is controlled directly by this class for discrete events.
/// I am avoiding using C# events because the direct imperative code is much easier to follow in
/// this case.
/// </remarks>
[ExecuteAlways]
public sealed class CharacterAnimationController : MonoBehaviour
{
    // TODO: Remove these! CharacterAnimationController should be created by Character with the
    // correct values
    [SerializeField] AnimatedSprite m_hairSprite;
    [SerializeField] AnimatedSprite m_skinSprite;
    [SerializeField] AnimatedSprite m_outlineSprite;

    public void BeginFireAnimation()
    {
        if (m_isOnFire)
            return;

        m_isOnFire = true;
        SetRestingTint(Color.red);

        m_fireAnimationCR = StartCoroutine(FireAnimationCR());
        m_fireAttachment?.Ignite();
    }

    public void EndFireAnimation()
    {
        if (!m_isOnFire)
            return;

        m_isOnFire = false;
        SetRestingTint(Color.white);

        StopCoroutine(m_fireAnimationCR);
        m_fireAttachment?.Extinguish();
    }

    public void TakeHit() => FlashWhite();

    public void RegisterFireAttachment(CharacterFireAttachment fireAttachment)
    {
        m_fireAttachment = fireAttachment;
    }

    IEnumerator FireAnimationCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            FlashWhite();
        }
    }

    void SetRestingTint(Color tint)
    {
        m_defaultTint = tint;
        if (!m_isFlashing)
        {
            m_characterSprite.MakeTinted(m_defaultTint);
        }
    }

    void FlashWhite()
    {
        StartCoroutine(FlashWhiteCR());
    }

    IEnumerator FlashWhiteCR()
    {
        if (m_isFlashing) yield break;
        m_isFlashing = true;

        try
        {
            m_characterSprite.MakePureColor(Color.white);
            yield return new WaitForSeconds(0.1f);
            m_characterSprite.MakeTinted(m_defaultTint);
        }
        finally
        {
            m_isFlashing = false;
        }
    }

    void LateUpdate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            m_characterSprite.ShowAnimationFrame("Walk", 0, CharacterDirection.Down);
            return;
        }
#endif

        m_characterSprite.ShowAnimationFrame("Walk", (int)m_animationFrame, m_character.Direction);

        m_animationFrame += 10 * Time.deltaTime *
                Mathf.Clamp01(m_rigidbody2D.velocity.magnitude / m_character.MaxSpeed);

        if (m_animationFrame >= SpriteAnimation.FrameCountInAnimation("Walk"))
        {
            m_animationFrame -= SpriteAnimation.FrameCountInAnimation("Walk");
        }
    }

    void Awake()
    {
        m_character = GetComponent<Character>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
#if UNITY_EDITOR
        if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this)) return;
#endif
        m_characterSprite = CharacterSprite.Create("CharacterSprite", transform);
        m_characterSprite.SetHairSprite(m_hairSprite);
        m_characterSprite.SetSkinSprite(m_skinSprite);
        m_characterSprite.SetOutlineSprite(m_outlineSprite);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this)) return;
        UnityEditor.EditorApplication.delayCall += () =>
        {
            // Weird, but can happen when changing scenes in Play Mode in the editor
            if (this == null) return;
            if (m_characterSprite == null)
            {
                m_characterSprite = CharacterSprite.Create("CharacterSprite", transform);
            }

            m_characterSprite.SetHairSprite(m_hairSprite);
            m_characterSprite.SetSkinSprite(m_skinSprite);
            m_characterSprite.SetOutlineSprite(m_outlineSprite);
        };
    }
#endif

    Character m_character;
    Rigidbody2D m_rigidbody2D;
    CharacterSprite m_characterSprite;

    float m_animationFrame = 0;

    // Flashing white on damage / tinting red for fire
    Color m_defaultTint = Color.white;
    bool m_isFlashing = false;

    // Fire
    CharacterFireAttachment m_fireAttachment;
    Coroutine m_fireAnimationCR;
    bool m_isOnFire;
}
