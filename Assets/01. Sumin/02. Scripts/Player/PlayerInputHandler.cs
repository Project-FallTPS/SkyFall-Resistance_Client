using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    // 스탯 정보
    // 입력 받기
    [Header("# Component")]
    private PlayerAttackHandler _playerAttackHandler;
    private PlayerMovement _playerMovement;

    private float _h;
    private float _v;

    private bool _isKeyDown = false;

    private void Awake()
    {
        _playerAttackHandler = GetComponent<PlayerAttackHandler>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        GetMoveInput();
        _playerAttackHandler.Anim.ResetTrigger("anim_Player_Trigger_MeleeAttack");
        GetAttackInput();
    }

    private void FixedUpdate()
    {
        _playerMovement.HandleMovement(_h, _v, _isKeyDown);
    }

    private void GetAttackInput()
    {
        _isKeyDown = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        if(Input.GetMouseButton(0))
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
        _h = Input.GetAxis("Horizontal"); // A/D
        _v = Input.GetAxis("Vertical");   // W/S
    }

    public void TakeDamage(float damage)
    {
    }
}