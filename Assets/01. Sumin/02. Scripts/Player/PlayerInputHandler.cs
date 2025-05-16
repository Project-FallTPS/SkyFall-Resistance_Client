using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    // ���� ����
    // �Է� �ޱ�
    [Header("# Component")]
    private PlayerAttackHandler _playerAttackHandler;

    private void Awake()
    {
        _playerAttackHandler = GetComponent<PlayerAttackHandler>();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //_playerAttackHandler.PerformAttack();
        }
    }

    public void TakeDamage(float damage)
    {
    }
}