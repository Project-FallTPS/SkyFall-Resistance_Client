using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VFX : MonoBehaviour
{
    private ParticleSystem[] _particleSystems;

    private void Awake()
    {
        _particleSystems = GetComponentsInChildren<ParticleSystem>(true); // �ڽ� ���� ��ü ��������
    }
    public void OnGetFromPool(Vector3 position)
    {
        transform.position = position;
        transform.rotation = Quaternion.identity;
        StartCoroutine(VFXCoroutine());
    }

    public void OnGetFromPool(Vector3 position, Quaternion quaternion)
    {
        transform.position = position;
        transform.rotation = quaternion;
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

    }
}
