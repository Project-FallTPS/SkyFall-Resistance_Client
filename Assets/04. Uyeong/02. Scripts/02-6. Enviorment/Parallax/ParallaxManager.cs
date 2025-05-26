using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : Singleton<ParallaxManager>
{
    [Header("Layer 세팅")]
    [SerializeField]
    private List<ParallaxLayerSet> _parallaxLayerSets;
    private Dictionary<EParallaxLayerType, ParallaxLayerSet> _parallaxLayerSetDictionary = new Dictionary<EParallaxLayerType, ParallaxLayerSet>();

    protected override void Awake()
    {
        base.Awake();
        InitializeLayerSetDictionary();
    }

    private void OnValidate()
    {
        EnsureListInitialized();
        EnsureListCountMatchesEnum();
        SyncLayerTypesWithEnumOrder();
    }

    public ParallaxLayerSet GetParallaxLayerSet(EParallaxLayerType layerType)
    {
        return _parallaxLayerSetDictionary[layerType];
    }

    private void InitializeLayerSetDictionary()
    {
        for (int i = 0; i < (int)EParallaxLayerType.Count; i++)
        {
            _parallaxLayerSetDictionary[(EParallaxLayerType)i] = _parallaxLayerSets[i];
        }
    }

    private void EnsureListInitialized()
    {
        if (_parallaxLayerSets == null)
        {
            _parallaxLayerSets = new List<ParallaxLayerSet>();
        }
    }

    private void EnsureListCountMatchesEnum()
    {
        int enumCount = (int)EParallaxLayerType.Count;
        int enumIndex = 0;
        // 부족한 항목 추가
        while (_parallaxLayerSets.Count < enumCount)
        {
            _parallaxLayerSets.Add(new ParallaxLayerSet((EParallaxLayerType)enumIndex));
            enumIndex++;
        }

        // 불필요한 항목 제거
        if (_parallaxLayerSets.Count > enumCount)
        {
            _parallaxLayerSets.RemoveRange(enumCount, _parallaxLayerSets.Count - enumCount);
        }
    }

    private void SyncLayerTypesWithEnumOrder()
    {
        // LayerType 중복 방지 및 순서 고정
        for (int i = 0; i < _parallaxLayerSets.Count; i++)
        {
            _parallaxLayerSets[i].LayerType = (EParallaxLayerType)i;
        }
    }
}
