using UnityEngine;

[ExecuteAlways]
public class DynamicAimTarget : MonoBehaviour
{
    public float maxDistance = 30f;
    public LayerMask raycastLayers = ~0; // 기본: 모든 레이어
    public Color gizmoColor = Color.green;
    public float gizmoRadius = 0.1f;

    private void LateUpdate()
    {
        if (Camera.main == null) return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, raycastLayers))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = ray.origin + ray.direction * maxDistance;
        }

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
}
