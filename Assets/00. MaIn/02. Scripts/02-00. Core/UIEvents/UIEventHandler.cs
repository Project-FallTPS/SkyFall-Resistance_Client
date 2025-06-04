using System;
using UnityEngine;

public class UIEventHandler : Singleton<UIEventHandler>
{
    public Action<float, float> OnStaminaChange;
    public Action<float, float> OnHealthChange;
    public Action OnPlayerAttackHit;
    public Action<EWeaponType> OnPlayerWeaponChange;
    public Action OnPlayerDie;
    public Action OnBossDie;
}
