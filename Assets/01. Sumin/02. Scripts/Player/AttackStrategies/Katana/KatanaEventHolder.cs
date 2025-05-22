using UnityEngine;

public class KatanaEventHolder : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private GameObject _katana;
    private Collider _katanaCollider;

    private void Awake()
    {
        _katanaCollider = _katana.GetComponent<Collider>();
    }

    public void SetCollider(int flag)
    {
        _katanaCollider.enabled = flag == 1 ? true : false;
        if (flag == 1)
        {
            PlayerEffectPoolManager.Instance.GetObject(EPlayerEffectType.SwrodSlash1, _katana.transform.position, _katana.transform.rotation);
        }
    }
}