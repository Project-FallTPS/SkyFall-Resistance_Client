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
            receiver.ReceiveAccessory(_type, gameObject);
            SetEquipped(true);
        }
    }

    public void Excecute()
    {
        //사거리 증가를 어떻게 박아 볼까요?
    }

    public void SetEquipped(bool flag)
    {
        _collider.enabled = !flag;
    }
}