using System;

[Serializable]
public class ParallaxLayerSet
{
    public ParallaxLayerSet
        (
        EParallaxLayerType layerType,
        float moveSpeed = 0f,
        float startPositionY = 0f,
        float loopOffset = 1000f,
        bool isLooping = false
        )
    {
        LayerType = layerType;
        MoveSpeed = moveSpeed;
        StartPositionY = startPositionY;
        LoopOffset = loopOffset;
        IsLooping = isLooping;
    }

    public EParallaxLayerType LayerType;
    public float MoveSpeed;
    public float StartPositionY;
    public float LoopOffset;
    public bool IsLooping;
}