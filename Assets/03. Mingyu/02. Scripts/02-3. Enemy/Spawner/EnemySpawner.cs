using UnityEngine;

public class EnemySpawner : Spawner<SpawnedObjectInfo<EEnemyType>, EEnemyType>
{
    protected override void Spawn()
    {
        EnemyPoolManager.Instance.GetObject(PickRandomObject(), SetRandomSpawnPosition());
    }
    protected override EEnemyType PickRandomObject()
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
        return EEnemyType.Shooting;
    }
}
