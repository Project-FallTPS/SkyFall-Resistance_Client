using UnityEngine;

public class Acc_KatanaAttackRange : MonoBehaviour, IAccessory
{
    [SerializeField] private EAccessoryType _type;
    public EAccessoryType Type => _type;

    public AccessoryData Data { get; private set; }
    private Collider _collider;
    private bool _isEqiupped = false;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        Data = AccessoryManager.Instance.GetData(_type);
    }

    private void OnEnable()
    {
        SetEquipped(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<IItemReceiver>(out var receiver))
        {
            SetEquipped(true);
            receiver.ReceiveAccessory(_type, gameObject);
        }
    }

    public void Execute()
    {
        if (_isEqiupped)
        {
            // PlayerAttackHandler에서 카타나를 찾아서 크기 조절
            var playerAttackHandler = FindFirstObjectByType<PlayerAttackHandler>();
            if (playerAttackHandler != null)
            {
                foreach (var weapon in playerAttackHandler.Weapons)
                {
                    if (weapon.name == "Katana")
                    {
                        // 현재 크기에 1.5배를 곱해서 확대
                        Vector3 currentScale = weapon.transform.localScale;
                        weapon.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z * 1.5f);
                        break;
                    }
                }
            }
        }
    }

    public void SetEquipped(bool flag)
    {
        _collider.enabled = !flag;
        _isEqiupped = flag;
    }
}