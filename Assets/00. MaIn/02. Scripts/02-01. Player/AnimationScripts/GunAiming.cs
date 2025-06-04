using UnityEngine;

public class GunAiming : MonoBehaviour
{
    public Transform aimTarget;
    public Animator animator;
    public string controlLayerName = "Shooting Layer";
    public string disableStateName = "Empty";

    void LateUpdate()
    {
        if (animator == null || aimTarget == null) return;

        int layerIndex = animator.GetLayerIndex(controlLayerName);
        if (layerIndex < 0) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layerIndex);
        if (state.IsName(disableStateName)) return;

        // 회전 적용
        transform.rotation = Quaternion.LookRotation(aimTarget.position - transform.position);
    }
}