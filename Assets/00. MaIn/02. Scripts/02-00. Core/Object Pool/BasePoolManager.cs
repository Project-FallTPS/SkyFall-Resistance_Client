using UnityEngine;
using System.Collections.Generic;
using System;

public abstract class BasePoolManager<TEnum, TPoolInfo> : Singleton<BasePoolManager<TEnum, TPoolInfo>>
    where TEnum : Enum
    where TPoolInfo : BasePoolInfo<TEnum>
{
    [SerializeField] protected List<TPoolInfo> _poolInfoList;

    // PoolList의 타입별 시작 위치
    private Dictionary<TEnum, int> _startIndexDictionary = new Dictionary<TEnum, int>();
    // PoolList의 타입별 개수
    private Dictionary<TEnum, int> _typeCountDictionary = new Dictionary<TEnum, int>();

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        _poolInfoList.Sort((a, b) => a.Type.CompareTo(b.Type));

        int index = 0;

        foreach (TPoolInfo info in _poolInfoList)
        {
            for (int i = 0; i < info.InitCount; i++)
            {
                info.PoolQueue.Enqueue(CreateNewObject(info));
            }

            if (_typeCountDictionary.ContainsKey(info.Type))
            {
                _typeCountDictionary[info.Type]++;
            }
            else
            {
                _typeCountDictionary[info.Type] = 1;
                _startIndexDictionary[info.Type] = index;
            }

            index++;
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
        if (!_startIndexDictionary.ContainsKey(type))
        {
            return null;
        }

        int startIndex = _startIndexDictionary[type];
        return _poolInfoList[startIndex];
    }

    private TPoolInfo GetPoolByType(TEnum type, int relativeIndex)
    {
        if (!_startIndexDictionary.ContainsKey(type))
        {
            return null;
        }

        int startIndex = _startIndexDictionary[type];

        if (relativeIndex >= _typeCountDictionary[type])
        {
            return null;
        }

        return _poolInfoList[startIndex + relativeIndex];
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

    public GameObject GetObject(TEnum type, Vector3 position, Quaternion rotation, System.Action<GameObject> beforeActivate = null)
    {
        TPoolInfo info = GetPoolByType(type);
        if (info == null) return null;

        GameObject obj = (info.PoolQueue.Count > 0) ? info.PoolQueue.Dequeue() : CreateNewObject(info);

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // 이 타이밍에 초기화
        beforeActivate?.Invoke(obj);

        obj.SetActive(true);
        return obj;
    }

    public GameObject GetObjectByRandom(TEnum type)
    {
        int randomIndex = UnityEngine.Random.Range(0, _typeCountDictionary[type]);
        TPoolInfo info = GetPoolByType(type, randomIndex);
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

    public GameObject GetObjectByRandom(TEnum type, Vector3 position)
    {
        int randomIndex = UnityEngine.Random.Range(0, _typeCountDictionary[type]);
        TPoolInfo info = GetPoolByType(type, randomIndex);
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

    public GameObject GetObjectByRandom(TEnum type, Vector3 position, Quaternion rotation)
    {
        int randomIndex = UnityEngine.Random.Range(0, _typeCountDictionary[type]);
        TPoolInfo info = GetPoolByType(type, randomIndex);
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

    public GameObject GetObjectByRandom(TEnum type, Vector3 position, Quaternion rotation, System.Action<GameObject> beforeActivate = null)
    {
        int randomIndex = UnityEngine.Random.Range(0, _typeCountDictionary[type]);
        TPoolInfo info = GetPoolByType(type, randomIndex);
        if (info == null) return null;

        GameObject obj = (info.PoolQueue.Count > 0) ? info.PoolQueue.Dequeue() : CreateNewObject(info);

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        // 이 타이밍에 초기화
        beforeActivate?.Invoke(obj);

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
