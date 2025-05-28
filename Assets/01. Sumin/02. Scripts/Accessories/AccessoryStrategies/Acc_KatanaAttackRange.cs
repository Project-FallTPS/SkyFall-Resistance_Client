using UnityEngine;

public class Acc_KatanaAttackRange : AccessoryBase, IAccessory
{
    public void Execute()
    {
        if (IsEqiupped)
        {
            // PlayerAttackHandler에서 카타나를 찾아서 크기 조절
            var playerAttackHandler = FindFirstObjectByType<PlayerAttackHandler>();
            if (playerAttackHandler != null)
            {
                foreach (var weapon in playerAttackHandler.Weapons)
                {
                    if (weapon.name == "Katana")
                    {
                        // 현재 크기에 1.5배를 곱해서 확대
                        Vector3 currentScale = weapon.transform.localScale;
                        weapon.transform.localScale = new Vector3(currentScale.x, currentScale.y, currentScale.z * 1.5f);
                        break;
                    }
                }
            }
        }
    }
}