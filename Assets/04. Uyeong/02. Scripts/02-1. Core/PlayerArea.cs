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
            // �÷��̾ ��� ������ �����ٰ� �˸��� ���
        }

    }
}
