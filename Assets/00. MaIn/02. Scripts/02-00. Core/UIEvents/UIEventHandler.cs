using System;
using UnityEngine;

public class UIEventHandler : Singleton<UIEventHandler>
{
    public Action<float, float> OnStaminaChange;
    public Action OnPlayerAttackHit;
    public Action<EWeaponType> OnPlayerWeaponChange;
}
