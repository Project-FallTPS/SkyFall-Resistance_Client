using UnityEngine;

public class Acc_RangeBomb : AccessoryBase, IAccessory
{
    private int _attackCount = 0;
    private PlayerAttackHandler _playerAttackHandler = null;

    public void OnAttack()
    {
        if (_playerAttackHandler == null)
        {
            _playerAttackHandler = GetComponentInParent<PlayerAttackHandler>();
            if (_playerAttackHandler == null) return;
        }

        if (_playerAttackHandler.CurrentStrategy is not RangeStrategy)
        {
            return;
        }

        // 추가 총알 발사
        Vector3 dir = SetDirection();
        Quaternion rot = Quaternion.LookRotation(dir);

        float spreadAngle = 10f; // 전체 퍼짐 각도
        int count = _attackCount;

        for (int i = 0; i < count; i++)
        {
            // 중심에서 좌우로 퍼지게 하기 위해 -spread/2 ~ +spread/2
            float angleOffset = Mathf.Lerp(-spreadAngle / 2f, spreadAngle / 2f, count == 1 ? 0.5f : (float)i / (count - 1));

            Quaternion spreadRot = Quaternion.AngleAxis(angleOffset, Vector3.forward) * rot;

            Vector3 dirWithSpread = spreadRot * Vector3.forward;

            GameObject bullet = BulletPoolManager.Instance.GetObject(
                EBulletType.PlayerExplodeBullet,
                transform.position,
                Quaternion.LookRotation(dirWithSpread),
                (obj) =>
                {
                    obj.GetComponent<IBullet>().SetStats (
                        AccessoryManager.Instance.GetData(Type).GetStatBonusData(EStatType.Damage),
                        dirWithSpread,
                        AccessoryManager.Instance.GetData(Type).GetStatBonusData(EStatType.ExplodeRange)
                    );
                });
        }
    }

    private Vector3 SetDirection()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("AimCube")))))
        {
            return (hitInfo.point - transform.position).normalized;
        }

        Vector3 fallbackPoint = ray.origin + ray.direction * 50f;
        return (fallbackPoint - transform.position).normalized;
    }

    public void OnEquip()
    {
        if (_playerAttackHandler == null)
        {
            _playerAttackHandler = GetComponentInParent<PlayerAttackHandler>();
        }
        _attackCount++;
    }

    public void OnHit(IDamageable target)
    {
    }
}