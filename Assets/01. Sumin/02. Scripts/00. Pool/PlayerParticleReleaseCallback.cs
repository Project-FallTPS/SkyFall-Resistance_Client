using UnityEngine;

public class PlayerParticleReleaseCallback : MonoBehaviour
{
    [SerializeField] private EPlayerEffectType _type;

    private void OnParticleSystemStopped()
    {
        PlayerEffectPoolManager.Instance.ReturnObject(gameObject, _type);
    }
}
