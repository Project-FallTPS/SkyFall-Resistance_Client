using UnityEngine;

public interface IBullet
{
    public void SetStats(float damage, Vector3 dir, float explodeRange = 0f);
}