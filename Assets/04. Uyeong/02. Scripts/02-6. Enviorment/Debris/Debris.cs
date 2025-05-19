using UnityEngine;

public abstract class Debris : MonoBehaviour, ILaunchable, IDamageable
{
    private EDebrisType _debrisType = EDebrisType.Normal;
    public EDebrisType DebrisType => _debrisType;

    private Rigidbody _rigidbody;

    private float _currentHealth = 10f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _debrisType = DefineType();
    }

    public void Launch(Vector3 direction, float magnitude)
    {
        _rigidbody.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            HandleDestruction();
        }
    }

    protected abstract EDebrisType DefineType();
    protected abstract void HandleDestruction();
    protected void Release()
    {
        DebrisPoolManager.Instance.ReturnObject(this.gameObject, _debrisType);
    }
}
