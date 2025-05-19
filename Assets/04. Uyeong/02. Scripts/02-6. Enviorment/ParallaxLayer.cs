using UnityEngine;
using static ParallaxManager;

public class ParallaxLayer : MonoBehaviour
{
    private Vector3 _startPosition;
    private float _moveSpeed = 5f;
    private float _playerAreaRadius;
    private float _loopOffset;
    private bool _isLooping = true;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
        if (_isLooping && transform.position.y >= _playerAreaRadius + _loopOffset)
        {
            transform.localPosition = _startPosition;
        }
    }

    public void Initialize(ParallaxLayerSet parallaxLayerSet, float playerAreaRadius)
    {
        _moveSpeed = parallaxLayerSet.MoveSpeed;
        _playerAreaRadius = playerAreaRadius;
        _loopOffset = parallaxLayerSet.LoopOffset;
        _isLooping = parallaxLayerSet.IsLooping;
    }
}
