using UnityEngine;
using UnityEngine.Animations.Rigging;

public class IKWeightController : MonoBehaviour
{
    public Animator animator;
    public Rig rig;
    public string rigControlLayer = "Shooting Layer";
    public string disableStateName = "Empty";

    void LateUpdate()
    {
        if (animator == null || rig == null) return;

        int layerIndex = animator.GetLayerIndex(rigControlLayer);
        if (layerIndex < 0) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layerIndex);
        rig.weight = state.IsName(disableStateName) ? 0f : 1f;
    }
}