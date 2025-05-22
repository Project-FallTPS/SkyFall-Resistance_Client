using UnityEngine;

public class ExplosiveDebris : Debris
{
    [SerializeField]
    private LayerMask _damagedLayer;
    [SerializeField]
    private float _explosionRange = 5f;
    [SerializeField]
    private float _damage = 5f;

    protected override EDebrisType DefineType() => EDebrisType.Explosive;

    protected override void HandleDestruction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRange, _damagedLayer);
        foreach (Collider collider in colliders)
        {
            IDamageable damagedObject = collider.GetComponent<IDamageable>();
            if (damagedObject == null)
            {
                return;
            }

            damagedObject.TakeDamage(_damage);
        }

        StartCoroutine(ReleaseAfterEffect());
    }
}
