using UnityEngine;

public class ExplosiveDebris : Debris
{
    [SerializeField]
    private LayerMask _damagedLayer;

    // 추가된 부분
    [Header("충돌 대상 (LayerName)")]
    [SerializeField]
    private string _explosionTriggerLayerName = "Ground";
    private LayerMask _explosionTriggerMask;

    [SerializeField]
    private float _explosionRange = 5f;
    [SerializeField]
    private float _damage = 5f;

    private void Awake()
    {
        int layer = LayerMask.NameToLayer(_explosionTriggerLayerName);
        if (layer == -1)
        {
            Debug.LogError($"Layer '{_explosionTriggerLayerName}' not found in project.");
        }
        _explosionTriggerMask = 1 << layer;
    }

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

        ReleaseAfterEffect();
    }

    protected override void HandleCollision(int layer)
    {
        if (((1 << layer) & _explosionTriggerMask) != 0)
        {
            HandleDestruction();
        }
    }
}
