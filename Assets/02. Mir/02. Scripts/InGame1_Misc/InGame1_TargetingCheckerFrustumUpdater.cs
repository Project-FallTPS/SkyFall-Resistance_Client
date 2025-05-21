using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class InGame1_TargetingCheckerFrustumUpdater : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxDistance = 30f;

    private MeshCollider _meshCollider;
    private Mesh _frustumMesh;

    void Awake()
    {
        if (_camera == null) _camera = Camera.main;
        _meshCollider = GetComponent<MeshCollider>();
        _frustumMesh = new Mesh();
        _meshCollider.sharedMesh = _frustumMesh;
        _meshCollider.convex = true;
        _meshCollider.isTrigger = true;
    }

    void LateUpdate()
    {
        if (!_camera) return;

        _frustumMesh.Clear();

        float fov = _camera.fieldOfView;
        float aspect = _camera.aspect;
        float near = _camera.nearClipPlane;
        float far = _maxDistance;

        float halfHeight = Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
        float nearHeight = near * halfHeight;
        float nearWidth = nearHeight * aspect;
        float farHeight = far * halfHeight;
        float farWidth = farHeight * aspect;

        Vector3[] verts = new Vector3[8];

        // Near plane corners
        verts[0] = new Vector3(-nearWidth, -nearHeight, near);
        verts[1] = new Vector3(nearWidth, -nearHeight, near);
        verts[2] = new Vector3(nearWidth, nearHeight, near);
        verts[3] = new Vector3(-nearWidth, nearHeight, near);

        // Far plane corners
        verts[4] = new Vector3(-farWidth, -farHeight, far);
        verts[5] = new Vector3(farWidth, -farHeight, far);
        verts[6] = new Vector3(farWidth, farHeight, far);
        verts[7] = new Vector3(-farWidth, farHeight, far);

        for (int i = 0; i < verts.Length; i++)
            verts[i] = _camera.transform.TransformPoint(verts[i] - _camera.transform.position); // local to world

        _frustumMesh.vertices = verts;

        _frustumMesh.triangles = new int[]
        {
            // Near plane
            0,1,2, 0,2,3,
            // Far plane
            4,6,5, 4,7,6,
            // Left
            0,3,7, 0,7,4,
            // Right
            1,5,6, 1,6,2,
            // Top
            3,2,6, 3,6,7,
            // Bottom
            0,4,5, 0,5,1
        };

        _frustumMesh.RecalculateNormals();
        _meshCollider.sharedMesh = _frustumMesh;
    }
}
