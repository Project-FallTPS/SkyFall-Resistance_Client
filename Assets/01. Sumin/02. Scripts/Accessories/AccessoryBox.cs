using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public struct AccClassMapper
{
    public EAccessoryType Type;
    public GameObject AccessoryClass;
}

public class AccessoryBox : MonoBehaviour
{
    public List<AccClassMapper> Mapper;
    public EAccessoryType Type;
    [SerializeField] GameObject[] AccessoryPrefabs;
    [SerializeField] private TextMeshPro _nameText;

    private Transform _player;

    private void Awake()
    {
        Array types = Enum.GetValues(typeof(EAccessoryType));
        Type = (EAccessoryType)types.GetValue(UnityEngine.Random.Range(0, (int)EAccessoryType.Count));
        _player = FindFirstObjectByType<PlayerMovement>().transform;
    }

    private void Update()
    {
        _nameText.transform.LookAt(_player);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.TryGetComponent<IItemReceiver>(out var receiver))
        {
            GameObject finalItem = null;

            foreach (var item in Mapper)
            {
                if (item.Type == Type)
                {
                    finalItem = item.AccessoryClass;
                    break;
                }
            }

            receiver.ReceiveAccessory(Type, finalItem);
        }
    }
}