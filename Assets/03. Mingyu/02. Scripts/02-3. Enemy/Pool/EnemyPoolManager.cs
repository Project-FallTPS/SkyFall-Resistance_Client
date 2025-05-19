using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : BasePoolManager<EEnemyType, EnemyPoolInfo>
{
    private HashSet<GameObject> _activeEnemies = new HashSet<GameObject>();
    public HashSet<GameObject> ActiveEnemies => _activeEnemies;
}