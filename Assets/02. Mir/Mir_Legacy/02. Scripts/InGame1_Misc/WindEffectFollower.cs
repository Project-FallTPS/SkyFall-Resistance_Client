using UnityEngine;

public class WindEffectFollower : MonoBehaviour
{
    [SerializeField] private Transform _cameraTargetTransform;

    private void Start()
    {
        if (_cameraTargetTransform == null) Debug.LogError("WindEffect's Target Is Not Assigned");
    }

    private void LateUpdate()
    {
        transform.position = _cameraTargetTransform.position;
    }
}
