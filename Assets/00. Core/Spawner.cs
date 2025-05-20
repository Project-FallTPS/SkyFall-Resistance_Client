using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Spawner<TInfo, TEnum> : MonoBehaviour 
    where TInfo : struct
    where TEnum : Enum
{
    [Header("Spawn Settings")]
    [SerializeField]
    protected List<TInfo> _spawnedObjects = new List<TInfo>();
    [SerializeField]
    protected float _spawnInterval;
    public float SpawnInterval { get => _spawnInterval; set => _spawnInterval = value; }

    [Header("Random Position Settings")]
    [SerializeField]
    protected float _spawnRadius;


    private bool _isSpawning = true;
    protected Coroutine _spawnCoroutine;

    private void Start()
    {
        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }
    private IEnumerator SpawnCoroutine()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(_spawnInterval);
            Spawn();
        }
    }
    protected abstract void Spawn();
    protected abstract TEnum PickRandomObject();
    protected Vector3 SetRandomSpawnPosition()
    {
        return transform.position + UnityEngine.Random.insideUnitSphere * _spawnRadius;
    }
}
