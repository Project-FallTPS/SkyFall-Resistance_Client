using UnityEngine;

public class KatanaEventHolder : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private GameObject _katanaObject;
    private Katana _katana;

    private void Awake()
    {
        _katana = _katanaObject.GetComponent<Katana>();
    }

    public void SetCollider(int flag) // 애니메이션 이벤트로 호출
    {
        if (flag == 1)
        {
            _katana.EnableAttack();
            PlayerEffectPoolManager.Instance.GetObject(EPlayerEffectType.KatanaAttackEffect, _katanaObject.transform.position, _katanaObject.transform.rotation);
        }
        else
        {
            _katana.DisableAttack();
        }
    }

    public void PlaySfx()
    {
        int idx = Random.Range(0, 3);
        SoundManager.Instance.PlaySfx((ESfxType)idx);
    }
}