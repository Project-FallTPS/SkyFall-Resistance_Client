using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class PlayerStatHolder : MonoBehaviour, IDamageable
{
    public GameObject GameObject => gameObject;

    [Header("# UI Event")]
    public MMF_Player OnHitEffect;
    [SerializeField] private UIDriftOnCamera driftScript;


    [Header("# Project")]
    [SerializeField] private PlayerStatCollectionSO _playerStatCollection; // 원본
    public Dictionary<EStatType, float> StatDict { get; private set; } // 캐싱

    private Animator _anim;
    private Rigidbody _rigid;
    private bool _isDead = false;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rigid = GetComponentInChildren<Rigidbody>();
        StatDict = _playerStatCollection.GetBaseStatDict();

        if (OnHitEffect == null) Debug.LogError("MMFeedBack is Not Assigned (GameFeel_Hit)");
    }

    private void Start()
    {
        //foreach(var perk in PerkManager.Instance.EquippedPerkBonuses)
        //{
        //    if(StatDict.ContainsKey(perk.Key))
        //    {
        //        StatDict[perk.Key] *= perk.Value;
        //    }
        //}    
    }

    public float GetStat(EStatType type)
    {
        return StatDict.TryGetValue(type, out var value) ? value : -1f;
    }

    public bool TryUseStamina(EStatType type)
    {
        if(type == EStatType.SprintStaminaUseRate)
        {
            if (StatDict[EStatType.CurrentStamina] < StatDict[type] * Time.fixedDeltaTime)
            {
                return false;
            }
            StatDict[EStatType.CurrentStamina] = Mathf.Max(0, StatDict[EStatType.CurrentStamina] - StatDict[type] * Time.deltaTime);
        }
        else if(type == EStatType.TargetDashStaminaUseRate)
        {
            if (StatDict[EStatType.CurrentStamina] < StatDict[type])
            {
                return false;
            }
            StatDict[EStatType.CurrentStamina] = Mathf.Max(0, StatDict[EStatType.CurrentStamina] - StatDict[type]);
        }
        else
        {
            return false;
        }

        UIEventHandler.Instance.OnStaminaChange?.Invoke(StatDict[EStatType.CurrentStamina], StatDict[EStatType.MaxStamina]);
        return true;
    }

    public void TakeDamage(float damage)
    {
        if(_isDead)
        {
            return;
        }

        StatDict[EStatType.Health] -= damage;

        OnHitEffect.PlayFeedbacks();
        driftScript.TemporarilyDisable(1f);  // 1초간 UI 드리프트 중지

        UIEventHandler.Instance.OnHealthChange?.Invoke(StatDict[EStatType.Health], StatDict[EStatType.MaxHealth]);

        SoundManager.Instance.PlaySfx(ESfxType.PlayerHit);

        Debug.Log($"플레이어 공격받음! {StatDict[EStatType.Health]}");

        if (StatDict[EStatType.Health] <= 0)
        {
            Die();
            _isDead = true;
        }
    }

    private void Die()
    {
        _anim.SetTrigger("anim_Player_Trigger_Die");

        DisableAllScriptsExceptThis();

        UIEventHandler.Instance.OnPlayerDie?.Invoke();
        _isDead = true;
    }

    private void DisableAllScriptsExceptThis()
    {
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this) // PlayerStatHolder 자신은 제외
            {
                script.enabled = false;
            }
        }
        _rigid.useGravity = true;
    }

    public void RegenStamina()
    {
        if (Mathf.Approximately(StatDict[EStatType.CurrentStamina], StatDict[EStatType.MaxStamina]))
        {
            return;
        }
        StatDict[EStatType.CurrentStamina] = Mathf.Min(StatDict[EStatType.MaxStamina], StatDict[EStatType.CurrentStamina] + StatDict[EStatType.StaminaRegenRate] * Time.deltaTime);
        UIEventHandler.Instance.OnStaminaChange?.Invoke(StatDict[EStatType.CurrentStamina], StatDict[EStatType.MaxStamina]);
    }
}