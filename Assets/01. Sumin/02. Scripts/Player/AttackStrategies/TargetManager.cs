using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class TargetManager : Singleton<TargetManager>
{
    [SerializeField] private Image _crossHair;
    public GameObject Target;

    private HashSet<GameObject> _enemies = new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            _enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            _enemies.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        SetTarget();
    }

    private void SetTarget()
    {
        GameObject target = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in _enemies)
        {
            float dist = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                target = enemy;
            }
        }

        Target = target;
        _crossHair.color = Target == null ? Color.white : Color.red;
    }
}
