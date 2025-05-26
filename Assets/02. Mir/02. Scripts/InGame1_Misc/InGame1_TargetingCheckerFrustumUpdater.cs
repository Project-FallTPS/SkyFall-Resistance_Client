using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class InGame1_TargetingCheckerFrustumFromUI : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private RectTransform _uiLockArea;
    [SerializeField] private float _maxDistance = 30f;

    private MeshCollider _meshCollider;
    private Mesh _frustumMesh;

    void Awake()
    {
        if (_camera == null) _camera = Camera.main;

        _meshCollider = GetComponent<MeshCollider>();
        _frustumMesh = new Mesh();
        _frustumMesh.name = "FrustumMesh";
        GenerateFrustumMesh();

        _meshCollider.sharedMesh = _frustumMesh;
        _meshCollider.convex = true;
        _meshCollider.isTrigger = true;
    }

    private void GenerateFrustumMesh()
    {
        float near = _camera.nearClipPlane;
        float far = _maxDistance;

        // UI 영역: 화면 기준 위치와 사이즈
        Vector2 uiCenter = _uiLockArea.position;
        Vector2 uiSize = _uiLockArea.rect.size;

        // Far plane에서 UI 사각형의 월드 위치
        Vector3 farBL = _camera.ScreenToWorldPoint(new Vector3(uiCenter.x - uiSize.x / 2f, uiCenter.y - uiSize.y / 2f, far));
        Vector3 farTR = _camera.ScreenToWorldPoint(new Vector3(uiCenter.x + uiSize.x / 2f, uiCenter.y + uiSize.y / 2f, far));
        Vector3 farSize = farTR - farBL;

        // Near plane 크기: 원근 비례
        float ratio = near / far;
        Vector3 nearSize = farSize * ratio;

        // 정점: 카메라 로컬 좌표계 기준
        Vector3[] verts = new Vector3[8];

        // Near plane
        verts[0] = new Vector3(-nearSize.x / 2, -nearSize.y / 2, near);
        verts[1] = new Vector3(nearSize.x / 2, -nearSize.y / 2, near);
        verts[2] = new Vector3(nearSize.x / 2, nearSize.y / 2, near);
        verts[3] = new Vector3(-nearSize.x / 2, nearSize.y / 2, near);

        // Far plane
        verts[4] = new Vector3(-farSize.x / 2, -farSize.y / 2, far);
        verts[5] = new Vector3(farSize.x / 2, -farSize.y / 2, far);
        verts[6] = new Vector3(farSize.x / 2, farSize.y / 2, far);
        verts[7] = new Vector3(-farSize.x / 2, farSize.y / 2, far);

        // 월드 좌표 변환
        for (int i = 0; i < verts.Length; i++)
            verts[i] = _camera.transform.TransformPoint(verts[i]);

        // 메쉬 설정
        _frustumMesh.vertices = verts;
        _frustumMesh.triangles = new int[]
        {
            0,1,2, 0,2,3, // Near
            4,6,5, 4,7,6, // Far
            0,3,7, 0,7,4, // Left
            1,5,6, 1,6,2, // Right
            3,2,6, 3,6,7, // Top
            0,4,5, 0,5,1  // Bottom
        };

        _frustumMesh.RecalculateNormals();
    }
}
