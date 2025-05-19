using UnityEngine;

public class Debris : MonoBehaviour, ILaunchable, IDamageable
{
    private Rigidbody _rigidbody;

    private float _currentHealth = 10f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float magnitude)
    {
        _rigidbody.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    public void TakeDamage(Damage damage)
    {
        _currentHealth -= damage.Value;

        if (_currentHealth <= 0)
        {
            HandleDestruction();
        }
    }

    protected void HandleDestruction()
    {

    }
}
