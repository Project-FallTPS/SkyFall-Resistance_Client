using UnityEngine;
using static ParallaxManager;

public class ParallaxLayer : MonoBehaviour
{
    private float _startPositionY;
    private float _moveSpeed = 5f;
    private float _loopOffset;
    private bool _isLooping = true;

    private void Update()
    {
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
        if (_isLooping && transform.position.y >= _loopOffset)
        {
            Vector3 position = transform.localPosition;
            position.y = _startPositionY;
            transform.localPosition = position;
        }
    }

    public void Initialize(ParallaxLayerSet parallaxLayerSet)
    {
        _moveSpeed = parallaxLayerSet.MoveSpeed;
        _startPositionY = parallaxLayerSet.StartPositionY;
        _loopOffset = parallaxLayerSet.LoopOffset;
        _isLooping = parallaxLayerSet.IsLooping;
    }
}
