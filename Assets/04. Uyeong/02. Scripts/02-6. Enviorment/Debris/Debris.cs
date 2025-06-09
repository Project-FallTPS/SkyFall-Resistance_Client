using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public abstract class Debris : MonoBehaviour, ILaunchable, IDamageable
{
    private EDebrisType _debrisType = EDebrisType.Normal;
    public EDebrisType DebrisType => _debrisType;

    private Rigidbody _rigidbody;

    [Header("체력")]
    [SerializeField]
    private float _maxHealth = 10f;
    private float _currentHealth = 10f;

    [Header("최대 속력")]
    [SerializeField]
    private float _maxSpeed = 2.5f;

    [Header("반환 거리")]
    [SerializeField]
    private float _releaseOffset = 50f;

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
    private string _canSpawnName = "CanSpawn";
    private float _fireSpeed;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _fireTrail = GetComponentInChildren<VisualEffect>();

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

        if (transform.position.y <= -_releaseOffset)
        {
            ReleaseAfterEffect();
        }

        Vector3 fireVelocity = -_rigidbody.linearVelocity.normalized * _fireSpeed;
        _fireTrail.SetVector3(_smokeVelocityName, fireVelocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(ETags.PlayerArea)))
        {
            _rigidbody.maxLinearVelocity = _maxSpeed;
        }
    }

    public void Initialize()
    {
        _meshObject.SetActive(true);

        _fireTrail.SetBool(_canSpawnName, true);

        _currentHealth = _maxHealth;
        _releaseTimer = 0f;
    }

    public void Launch(Vector3 direction, float magnitude)
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.one;
        _rigidbody.AddForce(direction * magnitude, ForceMode.Impulse);
    }

    public GameObject GameObject => gameObject;

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

        SoundManager.Instance.PlaySfx(ESfxType.Explosion1, transform.position);

        _fireTrail.SetBool(_canSpawnName, false);

        EVFXType vfxType = (EVFXType)((int)EVFXType.NormalDebrisExplosion + (int)DebrisType);
        GameObject vfx = VFXPoolManager.Instance.GetObjectByRandom(vfxType, transform.position, Quaternion.identity);
        vfx.GetComponent<VFX>().PlayVFX();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.gameObject.layer);
    }

    // 템플릿 메서드: 하위 클래스가 override 가능
    protected virtual void HandleCollision(int layer)
    {

    }
}
