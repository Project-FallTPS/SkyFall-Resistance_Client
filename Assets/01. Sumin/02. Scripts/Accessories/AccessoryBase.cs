using UnityEngine;

public abstract class AccessoryBase : MonoBehaviour
{
    [SerializeField] private EAccessoryType _type;
    public EAccessoryType Type => _type;

    public AccessoryData Data { get; private set; }
    private Collider _collider;
    public bool IsEqiupped { get; protected set; }

    protected virtual void Awake()
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

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(IsEqiupped)
        {
            return;
        }

        if (other.CompareTag("Player") && other.TryGetComponent<IItemReceiver>(out var receiver))
        {
            SetEquipped(true);
            receiver.ReceiveAccessory(_type, gameObject);
        }
    }

    public virtual void SetEquipped(bool flag)
    {
        _collider.enabled = !flag;
        IsEqiupped = flag;
    }
}