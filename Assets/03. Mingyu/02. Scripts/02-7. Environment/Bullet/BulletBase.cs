using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] 
    protected float _speed = 10f;
    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }
    
    [SerializeField] 
    protected float _lifeTime = 5f;
    
    public float LifeTime
    {
        get => _lifeTime;
        set => _lifeTime = value;
    }
    
    [SerializeField]
    protected float _damage = 10f;

    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }
    
    [SerializeField] 
    protected EDamageableType _damageableType;
    public EDamageableType DamageableType
    {
        get => _damageableType;
        set => _damageableType = value;
    }

    protected virtual void OnEnable()
    {
        StartCoroutine(LifeCycle());
    }

    protected virtual void Update()
    {
        Move();
    }
    
    protected abstract void OnTriggerEnter(Collider other);
    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    protected abstract void Move();

    protected virtual IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(_lifeTime);
        DamageablePoolManager.Instance.ReturnObject(gameObject, _damageableType);
    }

}
