using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField]
    private EParallaxLayerType _parallaxLayerType;

    private ParallaxLayerSet _parallaxLayerSet;

    private void Start()
    {
        ParallaxLayerSet parallaxLayerSet = ParallaxManager.Instance.GetParallaxLayerSet(_parallaxLayerType);
        Initialize(parallaxLayerSet);

        if (_parallaxLayerType == EParallaxLayerType.Near)
        {
            AdjustNearLayerPositionAndParticle();
        }
    }

    private void Update()
    {
        transform.position += Vector3.up * _parallaxLayerSet.MoveSpeed * Time.deltaTime;
        if (_parallaxLayerSet.IsLooping && transform.position.y >= _parallaxLayerSet.LoopOffset)
        {
            Vector3 position = transform.localPosition;
            position.y = _parallaxLayerSet.StartPositionY;
            transform.localPosition = position;
        }
    }

    private void Initialize(ParallaxLayerSet parallaxLayerSet)
    {
        _parallaxLayerSet = parallaxLayerSet;
    }

    private void AdjustNearLayerPositionAndParticle()
    {
        // WaveManager에서 시간 받아와서 거리 측정
        float waveTotalTime = WaveManager.Instance.FallSceneDuration;
        float length = waveTotalTime * _parallaxLayerSet.MoveSpeed;

        // 위치 내리기
        Vector3 position = transform.position;
        position.y = -length;
        transform.position = position;

        // particleSystem 길이 수정
        ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
        if (particleSystem == null)
        {
            return;
        }

        var shape = particleSystem.shape;
        
        if (shape.shapeType == ParticleSystemShapeType.Cone)
        {
            shape.length = length;
        }
        else if (shape.shapeType == ParticleSystemShapeType.Box)
        {
            Vector3 box = shape.scale;
            box.y = length;
            shape.scale = box;
        }
    }
}
