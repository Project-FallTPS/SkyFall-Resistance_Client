using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
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
    private float _releaseTime = 3f;        // Fire Trail이 꺼질 때까지의 시간
    private float _releaseTimer = 0f;       // Fire Trail이 꺼질 때까지의 타이머

    private bool _isBeingReleased = false;

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
        if (_isBeingReleased)
        {
            _releaseTimer += Time.deltaTime;
            if (_releaseTimer >= _releaseTime)
            {
                Release();
            }
            return;
        }

        if (transform.position.y <= -(_playerAreaRadius + _releaseOffset))
        {
            ReleaseAfterEffect();
        }

        _fireTrail.SetVector3(_spawnPositionName, transform.position);
        Vector3 fireVelocity = -_rigidbody.linearVelocity.normalized * _fireSpeed;
        _fireTrail.SetVector3(_smokeVelocityName, fireVelocity);
    }

    public void Initialize()
    {
        _meshObject.SetActive(true);

        _fireTrail.SetBool(_canSpawnName, true);

        _releaseTimer = 0f;
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
        _isBeingReleased = false;
        DebrisPoolManager.Instance.ReturnObject(this.gameObject, _debrisType);
    }

    protected void ReleaseAfterEffect()
    {
        _isBeingReleased = true;

        if (_meshObject != null)
        {
            _meshObject.SetActive(false);
        }

        _fireTrail.SetBool(_canSpawnName, false);

        EVFXType vfxType = (EVFXType)((int)EVFXType.NormalDebrisExplosion + (int)DebrisType);
        GameObject vfx = VFXPoolManager.Instance.GetObjectByRandom(vfxType, transform.position, Quaternion.identity);
        vfx.GetComponent<VFX>().PlayVFX();
    }
}
