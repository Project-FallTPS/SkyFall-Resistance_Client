using System;

[Serializable]
public class ParallaxLayerSet
{
    public ParallaxLayer ParallaxLayer;
    public float MoveSpeed;
    public float StartPositionY = -30f;
    public float LoopOffset = 10f;
    public bool IsLooping = true;
}