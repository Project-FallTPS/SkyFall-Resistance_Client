using UnityEngine;

public class KatanaEventHolder : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private Collider _katanaCollider;

    public void SetCollider(int flag)
    {
        _katanaCollider.enabled = flag == 1 ? true : false;
    }
}