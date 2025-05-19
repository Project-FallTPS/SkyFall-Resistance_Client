using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerStatHolder : MonoBehaviour
{
    [Header("# Project")]
    [SerializeField] private PlayerStatCollectionSO _playerStatCollection; // ¿øº»
    public Dictionary<EStatType, float> StatDict { get; private set; } // Ä³½Ì

    private void Awake()
    {
        StatDict = _playerStatCollection.GetBaseStatDict();
        PerkManager.Instance.CalculateFinalStats(StatDict);
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
        if(type == EStatType.SprintStaminaDrainRate)
        {
            if (StatDict[EStatType.CurrentStamina] < StatDict[type] * Time.deltaTime)
            {
                return false;
            }
            StatDict[EStatType.CurrentStamina] = Mathf.Max(0, StatDict[EStatType.CurrentStamina] - StatDict[type] * Time.deltaTime);
        }
        else if(type == EStatType.TargetDashStaminaDrainRate)
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
}