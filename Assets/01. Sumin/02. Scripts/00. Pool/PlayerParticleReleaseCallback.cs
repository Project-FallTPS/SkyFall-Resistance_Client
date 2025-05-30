using UnityEngine;

public class PlayerParticleReleaseCallback : MonoBehaviour
{
    [SerializeField] private EPlayerEffectType _type;
    private ParticleSystem _particle;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _particle.Play();
    }

    private void OnParticleSystemStopped()
    {
        PlayerEffectPoolManager.Instance.ReturnObject(gameObject, _type);
    }
}
