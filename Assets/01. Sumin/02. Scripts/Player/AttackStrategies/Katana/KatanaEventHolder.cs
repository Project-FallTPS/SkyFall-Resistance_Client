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

    public void SetCollider(int flag) // 애니메이션 이벤트로 호출
    {
        _katanaCollider.enabled = flag == 1 ? true : false;
        if (flag == 1)
        {
            PlayerEffectPoolManager.Instance.GetObject(EPlayerEffectType.MeleeAttackEffect, _katana.transform.position, _katana.transform.rotation);
        }
    }
}