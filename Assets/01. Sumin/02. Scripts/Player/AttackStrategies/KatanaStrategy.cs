using UnityEngine;
using UnityEngine.Rendering.Universal;

public class KatanaStrategy : IWeaponStrategy
{
    private WeaponData _weaponData;
    private GameObject _player;

    [Header("# Target Dash")]
    private float _dashDuration = 0.1f;
    private float _dashTimer = 0f;
    private bool _isDashing = false;
    private Vector3 _dashStartPos;
    private Vector3 _dashTargetPos;

    public KatanaStrategy(GameObject player)
    {
        _weaponData = WeaponManager.Instance.GetWeaponData(EWeaponType.Katana);
        _player = player;
    }

    public float GetDamage()
    {
        float baseDamage = _weaponData.Damage;
        float bonus = PlayerStatManager.Instance.GetStat(EStatType.Damage);
        return baseDamage * bonus;
    }

    public float GetAttackSpeed()
    {
        float baseSpeed = _weaponData.CoolTime;
        float bonus = PlayerStatManager.Instance.GetStat(EStatType.AttackSpeed);
        return baseSpeed * bonus;
    }

    public void Attack(IDamageable target)
    {
        if (target is MonoBehaviour mb)
        {
            StartDash(mb.gameObject);
        }
    }

    public void Attack(GameObject target)
    {
        StartDash(target);
    }

    public void StartDash(GameObject target)
    {
        if (_isDashing || target == null) return;

        _dashStartPos = _player.transform.position;
        _dashTargetPos = target.transform.position;

        _dashTimer = 0f;
        _isDashing = true;
    }

    public void Update()
    {
        if (_isDashing)
        {
            _dashTimer += Time.deltaTime;
            float t = _dashTimer / _dashDuration;

            if (t >= 1f)
            {
                // 도착 완료
                _player.GetComponent<CharacterController>().Move(_dashTargetPos - _player.transform.position); // 마지막 위치 보정
                _isDashing = false;
                Debug.Log(GetDamage());
            }
            else
            {
                // 선형 보간된 다음 위치 계산
                Vector3 nextPos = Vector3.Lerp(_dashStartPos, _dashTargetPos, t);
                Vector3 moveDelta = nextPos - _player.transform.position;
                _player.GetComponent<CharacterController>().Move(moveDelta);
            }
        }
    }
}
