using System;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : Singleton<ParallaxManager>
{
    [SerializeField]
    private SphereCollider _playerArea;

    [Serializable]
    public class ParallaxLayerSet
    {
        public ParallaxLayer ParallaxLayer;
        public float MoveSpeed;
        public float LoopOffset = 10f;
        public bool IsLooping = true;
    }

    [SerializeField]
    private List<ParallaxLayerSet> _environmentLayers = new List<ParallaxLayerSet>();

    private void Start()
    {
        float playerAreaRadius = _playerArea.radius;
        foreach (ParallaxLayerSet parallaxLayerSet in _environmentLayers)
        {
            parallaxLayerSet.ParallaxLayer.Initialize(parallaxLayerSet, playerAreaRadius);
        }
    }
}
