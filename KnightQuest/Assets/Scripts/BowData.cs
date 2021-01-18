using UnityEngine;

sealed class BowData : MonoBehaviour
{
    public ArrowSpawner arrowSpawner;
    public float arrowSpeed = 5;
    public float arrowLiveTime = 0.3f;

    [Tooltip("The time it takes to fully draw the bow, in seconds.")]
    public float chargeTime = 1;

    public BowParticles bowParticlesPrefab;

    public AudioSource releaseAudioSource;
}
