using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private float _moveSpeed = 5f;

    private void Update()
    {
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }
}
