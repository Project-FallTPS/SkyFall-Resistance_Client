using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Debris : MonoBehaviour, ILaunchable, IDamageable
{
    private EDebrisType _debrisType = EDebrisType.Normal;
    public EDebrisType DebrisType => _debrisType;

    private Rigidbody _rigidbody;

    private float _currentHealth = 10f;
    private float _playerAreaRadius;
    public float PlayerAreaRadius { set => _playerAreaRadius = value; }

    [Header("반환 거리")]
    [SerializeField]
    private float _releaseOffset = 10f;

    [Header("반환 시간")]
    [SerializeField]
    private float _releaseTime = 3f;

    [Header("자식 Mesh 오브젝트")]
    [SerializeField]
    private GameObject _meshObject;

    private VisualEffect _fireTrail;
    private string _smokeVelocityName = "SmokeVelocity";
    private string _spawnPositionName = "SpawnPosition";
    private string _canSpawnName = "CanSpawn";
    private float _fireSpeed;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _fireTrail = GetComponentInChildren<VisualEffect>();

        _fireTrail.SetVector3(_spawnPositionName, transform.position);
        _fireSpeed = _fireTrail.GetVector3(_smokeVelocityName).magnitude;

        _debrisType = DefineType();
    }

    private void Update()
    {
        if (transform.position.y <= -(_playerAreaRadius + _releaseOffset))
        {
            StartCoroutine(ReleaseAfterEffect());
        }

        _fireTrail.SetVector3(_spawnPositionName, transform.position);
        Vector3 fireVelocity = -_rigidbody.linearVelocity.normalized * _fireSpeed;
        _fireTrail.SetVector3(_smokeVelocityName, fireVelocity);
    }

    public void Initialize()
    {
        _meshObject.SetActive(true);
        _fireTrail.SetBool(_canSpawnName, true);
    }

    public void Launch(Vector3 direction, float magnitude)
    {
        _rigidbody.linearVelocity = Vector3.zero;
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

    protected IEnumerator ReleaseAfterEffect()
    {
        if (_meshObject != null)
        {
            _meshObject.SetActive(false);
        }

        _fireTrail.SetBool(_canSpawnName, false);

        yield return new WaitForSeconds(_releaseTime);

        DebrisPoolManager.Instance.ReturnObject(this.gameObject, _debrisType);
    }
}
