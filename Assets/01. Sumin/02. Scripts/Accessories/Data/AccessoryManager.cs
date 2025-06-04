using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccessoryManager : Singleton<AccessoryManager>
{

    [SerializeField] private AccessoryDataSO _dataSO;
    private static AccessoryDataSO _persistentDataSO;

    public Dictionary<EAccessoryType, ActiveAccessory> EquippedAccessories { get; private set; }
    public Dictionary<EAccessoryType, ActiveAccessory> SavedAccessories { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (_persistentDataSO == null)
        {
            _persistentDataSO = _dataSO;
            DontDestroyOnLoad(_dataSO);
        }
        else
        {
            _dataSO = _persistentDataSO;
        }

        if (EquippedAccessories == null)
        {
            EquippedAccessories = new Dictionary<EAccessoryType, ActiveAccessory>();
        }
        if (SavedAccessories == null)
        {
            SavedAccessories = new Dictionary<EAccessoryType, ActiveAccessory>();
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // 씬이 언로드될 때 현재 악세서리 정보 저장
        SaveCurrentAccessories();
    }

    public void SaveCurrentAccessories()
    {
        SavedAccessories.Clear();
        foreach (var kvp in EquippedAccessories)
        {
            SavedAccessories[kvp.Key] = new ActiveAccessory(kvp.Value.Data, kvp.Value.Object)
            {
                Count = kvp.Value.Count
            };
        }
    }

    public void RestoreAccessories(PlayerAttackHandler player)
    {
        foreach (var kvp in SavedAccessories)
        {
            var type = kvp.Key;
            var savedAcc = kvp.Value;
            
            // 저장된 악세서리 정보를 기반으로 새로운 악세서리 생성
            var accessoryObj = AccessoryPoolManager.Instance.GetObject(type);
            if (accessoryObj != null)
            {
                var accessory = accessoryObj.GetComponent<IAccessory>();
                if (accessory != null)
                {
                    // 저장된 악세서리의 카운트만큼 장착
                    for (int i = 0; i < savedAcc.Count; i++)
                    {
                        player.ReceiveAccessory(type, accessory);
                    }
                }
            }
        }
    }

    public void Equip(EAccessoryType type, IAccessory obj)
    {
        ActiveAccessory acc = null;

        if (!EquippedAccessories.ContainsKey(type))
        {
            acc = new ActiveAccessory(GetData(obj.Type), obj);
            acc.Count = 1;
            EquippedAccessories.Add(type, acc);

            obj.OnEquip();
            Debug.Log($"새거 {acc}");
        }
        else
        {
            acc = EquippedAccessories[type];
            acc.Count++;

            // 같은 오브젝트면 반납하지 않음
            if (acc.Object != obj)
            {
                Debug.Log($"원래거 {acc}");
                MonoBehaviour mono = obj as MonoBehaviour;
                if (mono != null)
                {
                    AccessoryPoolManager.Instance.ReturnObject(mono.gameObject, type);
                }
            }
            acc.Object.OnEquip(); // 기존 것에 대해 다시 호출
        }
    }

    public void UnEquip(EAccessoryType type)
    {
        if (EquippedAccessories.ContainsKey(type))
        {
            ActiveAccessory acc = EquippedAccessories[type];
            acc.Count--;
            if(acc.Count <= 0)
            {
                EquippedAccessories.Remove(type);
            }
        }
    }

    public bool IsEquipped(EAccessoryType type)
    {
        return EquippedAccessories.ContainsKey(type);
    }

    public ActiveAccessory GetAccessory(EAccessoryType type)
    {
        if (EquippedAccessories.TryGetValue(type, out var acc))
        {
            return acc;
        }
        else return null;
    }

    public AccessoryData GetData(EAccessoryType type)
    {
        return _dataSO.GetData(type);
    }

    public List<ActiveAccessory> GetEquippedAccessories(EWeaponType type)
    {
        var filtered = new List<ActiveAccessory>();

        foreach (var accessory in EquippedAccessories.Values)
        {
            string name = accessory.Data.Type.ToString();
            if (name.StartsWith(type.ToString()))
            {
                filtered.Add(accessory);
            }
        }

        return filtered;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "LobbyScene":
                Destroy(gameObject);
                break;
        }
    }
}
