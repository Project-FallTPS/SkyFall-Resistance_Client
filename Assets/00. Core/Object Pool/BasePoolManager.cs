using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class BasePoolManager<TEnum, TPoolInfo> : Singleton<BasePoolManager<TEnum, TPoolInfo>>
    where TEnum : Enum
    where TPoolInfo : BasePoolInfo<TEnum>
{
    [SerializeField] protected List<TPoolInfo> _poolInfoList;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        foreach (TPoolInfo info in _poolInfoList)
        {
            for (int i = 0; i < info.InitCount; i++)
            {
                info.PoolQueue.Enqueue(CreateNewObject(info));
            }
        }
    }

    private GameObject CreateNewObject(TPoolInfo info)
    {
        GameObject newObject = Instantiate(info.Prefab, info.Container.transform);
        newObject.SetActive(false);
        return newObject;
    }

    private TPoolInfo GetPoolByType(TEnum type)
    {
        foreach (TPoolInfo info in _poolInfoList)
        {
            if (type.Equals(info.Type))
            {
                return info;
            }
        }
        return null;
    }

    public GameObject GetObject(TEnum type)
    {
        TPoolInfo info = GetPoolByType(type);
        if (info == null) return null;

        GameObject obj;
        if (info.PoolQueue.Count > 0)
        {
            obj = info.PoolQueue.Dequeue();
        }
        else
        {
            obj = CreateNewObject(info);
        }
        obj.SetActive(true);
        return obj;
    }

    public GameObject GetObject(TEnum type, Vector3 position)
    {
        TPoolInfo info = GetPoolByType(type);
        if (info == null) return null;

        GameObject obj;
        if (info.PoolQueue.Count > 0)
        {
            obj = info.PoolQueue.Dequeue();
        }
        else
        {
            obj = CreateNewObject(info);
        }
        obj.transform.position = position;
        obj.SetActive(true);
        return obj;
    }

    public GameObject GetObject(TEnum type, Vector3 position, Quaternion rotation)
    {
        TPoolInfo info = GetPoolByType(type);
        if (info == null) return null;

        GameObject obj;
        if (info.PoolQueue.Count > 0)
        {
            obj = info.PoolQueue.Dequeue();
        }
        else
        {
            obj = CreateNewObject(info);
        }
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj, TEnum type)
    {
        TPoolInfo info = GetPoolByType(type);
        if (info == null) return;

        info.PoolQueue.Enqueue(obj);
        obj.SetActive(false);
    }
}
