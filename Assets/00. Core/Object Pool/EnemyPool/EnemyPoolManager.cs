using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolManager : BasePoolManager<EEnemyType, EnemyPoolInfo>
{
    private static EnemyPoolManager _instance;
    public static EnemyPoolManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindAnyObjectByType<EnemyPoolManager>();
            }
            return _instance;
        }
    }
}