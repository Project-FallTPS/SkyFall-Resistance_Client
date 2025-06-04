using UnityEngine;

public class GunFollower : MonoBehaviour
{
    public Transform handBone;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    void LateUpdate()
    {
        transform.position = handBone.position + handBone.rotation * positionOffset;
        transform.rotation = handBone.rotation * Quaternion.Euler(rotationOffset);
    }
}
