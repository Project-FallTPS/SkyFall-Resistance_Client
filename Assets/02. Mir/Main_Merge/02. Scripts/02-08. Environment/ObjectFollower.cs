using UnityEngine;

public class ObjectFollower : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;

    private void Start()
    {
        if (_targetObject == null) Debug.LogError("WindEffect do not have target");
    }

    private void Update()
    {
        transform.position = _targetObject.transform.position;
    }
}
