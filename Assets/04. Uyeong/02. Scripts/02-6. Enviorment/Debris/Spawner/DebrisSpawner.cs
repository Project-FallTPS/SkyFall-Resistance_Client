using UnityEngine;

public class DebrisSpawner : Spawner<SpawnedObjectInfo<EDebrisType>, EDebrisType>
{
    [Header("Æø¹ß Èû")]
    [SerializeField]
    private float _minExplosionForce = 3f;
    [SerializeField]
    private float _maxExplosionForce = 10f;

    // Test ¿ë
    public SphereCollider PlayerAreaCollider;
    private float _playerAreaRadius;

    private void Awake()
    {
        _playerAreaRadius = PlayerAreaCollider.radius;
    }

    protected override void Spawn()
    {
        GameObject debrisObject = DebrisPoolManager.Instance.GetObject(PickRandomObject(), SetRandomSpawnPosition());
        if (debrisObject == null)
        {
            return;
        }

        Debris debris = debrisObject.GetComponent<Debris>();
        if (debris == null)
        {
            return;
        }

        debris.Launch(Random.insideUnitSphere, Random.Range(_minExplosionForce, _maxExplosionForce));
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
