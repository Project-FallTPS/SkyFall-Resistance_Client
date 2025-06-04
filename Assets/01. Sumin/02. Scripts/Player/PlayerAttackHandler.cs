using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttackHandler : MonoBehaviour, IItemReceiver
{
    private const int MELEE_LAYER = 2;
    private const int RANGE_LAYER = 1;
    [Header("# Hierarchy")]
    [Header("# Weapon")]
    public List<GameObject> Weapons;

    [Header("# Stat")]
    public PlayerStatHolder PlayerStat { get; private set; }

    [Header("# Component")]
    public Animator Anim { get; private set; }
    public Rigidbody Rigid { get; private set; }

    private Dictionary<EWeaponType, IWeaponStrategy> _strategies = new Dictionary<EWeaponType, IWeaponStrategy>();

    private EWeaponType _currentWeapon;
    public IWeaponStrategy CurrentStrategy { get; private set; }

    private bool isInitializedAcc = false;

    private void Awake()
    {
        Rigid = GetComponentInChildren<Rigidbody>();
        Anim = GetComponentInChildren<Animator>();
        PlayerStat = GetComponent<PlayerStatHolder>();
        _strategies.Add(EWeaponType.Katana, new KatanaStrategy(this));
        _strategies.Add(EWeaponType.Range, new RangeStrategy(this));

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ChangeWeapon(EWeaponType.Katana);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LobbyScene")
        {
            Destroy(gameObject);
            return;
        }

        // 씬 로드가 완료된 후 악세서리 복원
        //RestoreAccessories();
    }

    private void RestoreAccessories()
    {
        if (AccessoryManager.Instance == null || AccessoryManager.Instance.SavedAccessories == null) return;

        // 저장된 악세서리 정보를 기반으로 복원
        foreach(var kvp in AccessoryManager.Instance.SavedAccessories)
        {
            var type = kvp.Key;
            var savedAcc = kvp.Value;
            
            // 저장된 개수만큼 반복
            for (int i = 0; i < savedAcc.Count; i++)
            {
                // 악세서리 오브젝트 생성
                GameObject accObj = AccessoryPoolManager.Instance.GetObject(type);
                if (accObj != null)
                {
                    // 오브젝트 활성화
                    accObj.SetActive(true);

                    // IAccessory 인터페이스 가져오기
                    var accessory = accObj.GetComponent<IAccessory>();
                    if (accessory != null)
                    {
                        // 직접 인터페이스를 전달하여 장착
                        ReceiveAccessory(type, accessory);
                    }
                }
            }
        }
        isInitializedAcc = true;
    }

    private void Update()
    {
        if (!isInitializedAcc)
        {
            RestoreAccessories();
        }
        CurrentStrategy?.Update();
    }

    public void ReceiveAccessory(EAccessoryType type, IAccessory accessory)
    {
        if(type.ToString().StartsWith(nameof(EWeaponType.Range)))
        {
            _strategies[EWeaponType.Range].AddAccessory(type, accessory);
        }
        else if(type.ToString().StartsWith(nameof(EWeaponType.Katana)))
        {
            _strategies[EWeaponType.Katana].AddAccessory(type, accessory);
        }
        //CurrentStrategy?.AddAccessory(type, accessory);
    }

    public void ChangeWeapon(EWeaponType type)
    {
        UIEventHandler.Instance.OnPlayerWeaponChange?.Invoke(type);

        if (_strategies.TryGetValue(type, out var strategy))
        {
            CurrentStrategy = strategy;
            _currentWeapon = type;
            
            switch(type)
            {
                case EWeaponType.Katana:
                    Anim.SetLayerWeight(RANGE_LAYER, 0f); // Range
                    Anim.SetLayerWeight(MELEE_LAYER, 1f); // Melee
                    break;
                case EWeaponType.Range:
                    Anim.SetLayerWeight(RANGE_LAYER, 1f);
                    Anim.SetLayerWeight(MELEE_LAYER, 0f);
                    break;
            }

            foreach(var weapon in Weapons)
            {
                if(weapon.name == type.ToString())
                {
                    weapon.SetActive(true);
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
    }

    public void PerformAttack()
    {
        CurrentStrategy?.Attack(TargetManager.Instance.Target);
    }
}