using UnityEngine;

public class MeleeDamage : MonoBehaviour, IAccessory
{
    [SerializeField] private EAccessoryType _type;
    public EAccessoryType Type => _type;
    public AccessoryData Data { get; private set; }
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        Data = AccessoryManager.Instance.GetData(_type);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<IItemReceiver>(out var receiver))
        {
            receiver.ReceiveAccessory(_type, gameObject);
            _collider.enabled = false;
        }
    }

    public void StatExecute(WeaponData data)
    {
        foreach (var bonus in Data.Bonuses)
        {
            //data.ModifyStat(bonus.StatType, bonus.Value);  // 스탯 변경 적용
        }
    }

    public void Excecute()
    {
        // 타격 시 발동하는 특수 효과가 있을 경우 여기에 구현
    }
}