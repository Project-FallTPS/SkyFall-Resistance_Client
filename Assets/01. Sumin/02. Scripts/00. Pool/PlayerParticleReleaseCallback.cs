using UnityEngine;

public class PlayerParticleReleaseCallback : MonoBehaviour
{
    public EPlayerEffectType Type;

    private void OnParticleSystemStopped()
    {
        PlayerEffectPoolManager.Instance.ReturnObject(gameObject, Type);
    }
}
