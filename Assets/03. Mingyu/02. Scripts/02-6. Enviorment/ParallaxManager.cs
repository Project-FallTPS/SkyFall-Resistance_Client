using System;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : Singleton<ParallaxManager>
{
    [Serializable]
    public class ParallaxLayerSet
    {
        public ParallaxLayer ParallaxLayer;
        public float MoveSpeed;
    }

    [SerializeField]
    private List<ParallaxLayerSet> _environmentLayers = new List<ParallaxLayerSet>();

    private void Start()
    {
        foreach (ParallaxLayerSet parallaxLayerSet in _environmentLayers)
        {
            parallaxLayerSet.ParallaxLayer.SetMoveSpeed(parallaxLayerSet.MoveSpeed);
        }
    }
}
