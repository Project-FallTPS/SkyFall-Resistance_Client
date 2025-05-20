using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    // 스탯 정보
    // 입력 받기
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
        GetMoveInput();
        GetAttackInput();
    }

    private void GetAttackInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _playerAttackHandler.PerformAttack();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerMovement.SetSprint(true);
            //_playerMovement.ChangeState(EPlayerMoveState.Sprint);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _playerMovement.SetSprint(false);
            //_playerMovement.ChangeState(EPlayerMoveState.Move);
        }
    }

    private void GetMoveInput()
    {
        float h = Input.GetAxis("Horizontal"); // A/D
        float v = Input.GetAxis("Vertical");   // W/S
        _playerMovement.HandleMovement(h, v);
    }

    public void TakeDamage(float damage)
    {
    }
}