using System.Collections;
using System.Collections.Generic;
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
public sealed class CharacterAnimationController : MonoBehaviour
{
    public void BeginFireAnimation()
    {
        if (m_isOnFire)
            return;

        m_isOnFire = true;

        foreach (var animator in m_animators)
        {
            animator.TintRedForFire();
        }

        m_fireAnimationCR = StartCoroutine(FireAnimationCR());
        m_fireAttachment?.Ignite();
    }

    public void EndFireAnimation()
    {
        if (!m_isOnFire)
            return;

        m_isOnFire = false;

        foreach (var animator in m_animators)
        {
            animator.UseNormalTint();
        }

        StopCoroutine(m_fireAnimationCR);
        m_fireAttachment?.Extinguish();
    }

    public void TakeHit()
    {
        foreach (var animator in m_animators)
        {
            animator.FlashWhite();
        }
    }

    /// <summary>
    /// Registers the <see cref="CharacterAnimator"/> to be controlled by this object.
    /// 
    /// Should only be called from within <see cref="CharacterAnimator"/>.
    /// </summary>
    public void RegisterCharacterAnimator(CharacterAnimator animator)
    {
        m_animators.Add(animator);
    }

    public void RegisterFireAttachment(CharacterFireAttachment fireAttachment)
    {
        m_fireAttachment = fireAttachment;
    }

    IEnumerator FireAnimationCR()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (var animator in m_animators)
            {
                animator.FlashWhite();
            }
        }
    }

    readonly HashSet<CharacterAnimator> m_animators = new HashSet<CharacterAnimator>();
    CharacterFireAttachment m_fireAttachment;
    Coroutine m_fireAnimationCR;
    bool m_isOnFire;
}
