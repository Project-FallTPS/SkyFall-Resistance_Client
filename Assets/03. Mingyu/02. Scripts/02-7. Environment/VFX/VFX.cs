using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VFX : MonoBehaviour
{
    [SerializeField]
    private EVFXType _vfxType;
    private ParticleSystem[] _particleSystems;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>(true); // �ڽ� ���� ��ü ��������
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

        // ��� ��ƼŬ�� �� ���� ������ ��ٸ�
        bool isAlive;
        do
        {
            isAlive = false;
            foreach (ParticleSystem ps in _particleSystems)
            {
                if (ps.IsAlive(true)) // �ڽ� ���� üũ
                {
                    isAlive = true;
                    break;
                }
            }
            yield return null;
        } while (isAlive);
        VFXPoolManager.Instance.ReturnObject(gameObject, _vfxType);
    }
}
