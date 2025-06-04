using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;

public class PlayerStatHolder : MonoBehaviour, IDamageable
{
    [Header("# UI Event")]
    public MMF_Player OnHitEffect;

    [Header("# Project")]
    [SerializeField] private PlayerStatCollectionSO _playerStatCollection; // 원본
    public Dictionary<EStatType, float> StatDict { get; private set; } // 캐싱

    private void Awake()
    {
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
            Debug.Log($"스프린트 스태미너 사용{StatDict[EStatType.CurrentStamina]}");
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
        StatDict[EStatType.Health] -= damage;

        OnHitEffect.PlayFeedbacks();

        if (StatDict[EStatType.Health] <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

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