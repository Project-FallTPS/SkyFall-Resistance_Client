using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VFX : MonoBehaviour
{
    [SerializeField]
    private EVFXType _vfxType;
    public EVFXType VFXType
    {
        get => _vfxType;
        set => _vfxType = value;
    }

    [SerializeField] 
    private bool _isPoolAble;
    private ParticleSystem[] _particleSystems;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>(true);
    }

    public void PlayVFX()
    {
        StartCoroutine(VFXCoroutine());
    }

    private IEnumerator VFXCoroutine()
    {
        foreach (ParticleSystem ps in _particleSystems)
        {
            ps.Play();
        }
        
        bool isAlive;
        do
        {
            isAlive = false;
            foreach (ParticleSystem ps in _particleSystems)
            {
                if (ps.IsAlive(true))
                {
                    isAlive = true;
                    break;
                }
            }
            yield return null;
        } while (isAlive);

        if (_isPoolAble)
        {
            VFXPoolManager.Instance.ReturnObject(gameObject, _vfxType);
        }
    }
}
