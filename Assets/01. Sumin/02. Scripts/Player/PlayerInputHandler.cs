using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    // ���� ����
    // �Է� �ޱ�
    [Header("# Component")]
    private PlayerAttackHandler _playerAttackHandler;
    private PlayerMovement _playerMovement;

    private Transform camTransform;

    private void Awake()
    {
        camTransform = Camera.main.transform;
        _playerAttackHandler = GetComponent<PlayerAttackHandler>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        //GetMoveInput();
        GetAttackInput();
    }

    private void GetAttackInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //_playerAttackHandler.PerformAttack();
        }
    }

    private void GetMoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        // ī�޶� �������� ���� ���

        Vector3 camForward = camTransform.forward;
        //camForward.y = 0f; // y �����Ͽ� ���� �̵���
        camForward.Normalize();

        Vector3 camRight = camTransform.right;
        //camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * v + camRight * h;

        //_playerMovement.SetMoveInput(moveDir);
    }

    public void TakeDamage(float damage)
    {
    }
}