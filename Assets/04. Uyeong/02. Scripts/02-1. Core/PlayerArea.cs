using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    private SphereCollider _playerZone;
    private float _playerZoneRadius;
    public float PlayerZoneRadius => _playerZoneRadius;

    private void Start()
    {
        _playerZone = GetComponent<SphereCollider>();
        _playerZoneRadius = _playerZone.radius;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(nameof(ETags.Player)))
        {
            // 플레이어가 경계 밖으로 나갔다고 알리는 기능
        }

    }
}
