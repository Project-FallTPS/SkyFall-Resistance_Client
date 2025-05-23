using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : Singleton<ParallaxManager>
{
    [Header("Near Layer")]
    [SerializeField]
    private ParallaxLayerSet _nearParallaxLayerSet;
    [SerializeField]
    private List<ParallaxLayer> _nearLayers;

    [Header("Middle Layer")]
    [SerializeField]
    private ParallaxLayerSet _middleParallaxLayerSet;
    [SerializeField]
    private List<ParallaxLayer> _middleLayers;

    [Header("Far Layer")]
    [SerializeField]
    private ParallaxLayerSet _farParallaxLayerSet;
    [SerializeField]
    private List<ParallaxLayer> _farLayers;

    [SerializeField]
    private List<ParallaxLayerSet> _environmentLayers = new List<ParallaxLayerSet>();

    private void Start()
    {
        foreach (ParallaxLayer nearLayer in _nearLayers)
        {
            nearLayer.Initialize(_nearParallaxLayerSet);
        }
        
    }
}
