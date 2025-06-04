using UnityEngine;

public interface IDamageable
{
    public GameObject GameObject { get; }
    public void TakeDamage(float damage);
}