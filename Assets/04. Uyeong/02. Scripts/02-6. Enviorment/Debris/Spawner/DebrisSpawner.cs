using UnityEngine;

public class DebrisSpawner : Spawner<SpawnedObjectInfo<EDebrisType>, EDebrisType>
{
    [Header("폭발 힘")]
    [SerializeField]
    private float _minExplosionForce = 500f;
    [SerializeField]
    private float _maxExplosionForce = 1000f;

    // Test 용
    public SphereCollider PlayerAreaCollider;
    private float _playerAreaRadius;

    private void Awake()
    {
        _playerAreaRadius = PlayerAreaCollider.radius;
    }

    protected override void Spawn()
    {
        GameObject debrisObject = DebrisPoolManager.Instance.GetObjectByRandom(PickRandomObject(), SetRandomSpawnPosition());
        if (debrisObject == null)
        {
            return;
        }

        Debris debris = debrisObject.GetComponent<Debris>();
        if (debris == null)
        {
            return;
        }

        debris.Initialize();
        
        Vector3 launchDirection = Random.insideUnitSphere;
        launchDirection.y = Mathf.Abs(launchDirection.y);
        debris.Launch(launchDirection, Random.Range(_minExplosionForce, _maxExplosionForce));
        debris.PlayerAreaRadius = _playerAreaRadius;
    }

    protected override EDebrisType PickRandomObject()
    {
        int randNum = Random.Range(0, 100);
        int probabilityPrefixSum = 0;

        foreach (var info in _spawnedObjects)
        {
            probabilityPrefixSum += info.Probability;
            if (randNum < probabilityPrefixSum)
            {
                return info.ObjectType;
            }
        }
        return EDebrisType.Normal;
    }
}
