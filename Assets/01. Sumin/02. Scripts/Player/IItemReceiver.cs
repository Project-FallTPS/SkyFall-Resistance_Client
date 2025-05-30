using UnityEngine;

public interface IItemReceiver
{
    public void ReceiveAccessory(EAccessoryType type, IAccessory accessory);
}
