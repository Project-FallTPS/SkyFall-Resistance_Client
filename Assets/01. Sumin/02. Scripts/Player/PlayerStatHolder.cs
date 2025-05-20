using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHolder : MonoBehaviour, IDamageable
{
    [Header("# Project")]
    [SerializeField] private PlayerStatCollectionSO _playerStatCollection; // ¿øº»
    public Dictionary<EStatType, float> StatDict { get; private set; } // Ä³½Ì

    private void Awake()
    {
        StatDict = _playerStatCollection.GetBaseStatDict();
        //PerkManager.Instance.CalculateFinalStats(StatDict);
    }

    private void Update()
    {
        RegenStamina();
    }

    public float GetStat(EStatType type)
    {
        return StatDict.TryGetValue(type, out var value) ? value : -1f;
    }

    public void UseStamina(float value)
    {
        StatDict[EStatType.CurrentStamina] = Mathf.Max(0, StatDict[EStatType.CurrentStamina] - value);
    }

    public bool TryUseStamina(EStatType type)
    {
        if(type == EStatType.SprintStaminaUseRate)
        {
            if (StatDict[EStatType.CurrentStamina] < StatDict[type] * Time.deltaTime)
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

        return true;
    }

    public void TakeDamage(float damage)
    {
        StatDict[EStatType.Health] -= damage;
        if (StatDict[EStatType.Health] <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

    }

    private void RegenStamina()
    {
        StatDict[EStatType.CurrentStamina] = Mathf.Min(StatDict[EStatType.MaxStamina], StatDict[EStatType.CurrentStamina] + StatDict[EStatType.StaminaRegenRate] * Time.deltaTime);
    }
}