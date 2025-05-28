using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
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
        GetInput();
    }

    private void FixedUpdate()
    {
        _playerMovement.HandleMovement(_h, _v, _isKeyDown);
    }

    private void GetInput()
    {
        _isKeyDown = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        if(Input.GetMouseButton(0))
        {
            _playerAttackHandler.PerformAttack();
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _playerMovement.SetSprint(true);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _playerMovement.SetSprint(false);
            Debug.Log("스프린트 해제");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _playerMovement.Jump();
        }
    }

    private void GetMoveInput()
    {
        _h = Input.GetAxis("Horizontal"); // A/D
        _v = Input.GetAxis("Vertical");   // W/S
    }
}