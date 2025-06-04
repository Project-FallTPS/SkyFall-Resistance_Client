using UnityEngine;

public class EnemyMaterialHandler : Singleton<EnemyMaterialHandler>
{
    private MaterialPropertyBlock _materialPropertyBlock;

    protected override void Awake()
    {
        base.Awake();
        _materialPropertyBlock = new MaterialPropertyBlock();
    }

    public void SetEnemyMaterialColor(SkinnedMeshRenderer skinnedMeshRenderer, Color color)
    {
        _materialPropertyBlock.SetColor("_Color", color);
        skinnedMeshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
