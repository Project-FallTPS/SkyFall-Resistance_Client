using UnityEngine;

public class BulletPoolManager : BasePoolManager<EBulletType, BulletPoolInfo>
{
    private static BulletPoolManager _instance;
    public static BulletPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<BulletPoolManager>();
            }
            return _instance;
        }
    }
}