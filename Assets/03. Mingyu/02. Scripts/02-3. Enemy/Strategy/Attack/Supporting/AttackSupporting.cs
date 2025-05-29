using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class AttackSupporting : IAttackStrategy, ITransitionStrategy
{
    public void Attack(EnemyController self)
    {
        ApplyShield(self);
    }

    public bool CanChangeToAttackState(EnemyController self)
    {
        return self.EnemyData.NextAttackableTime <= Time.time;
    }

    public bool CanChangeToTraceState(EnemyController self)
    {
        return true;
    }
    
    private void ApplyShield(EnemyController self)
    {
        Collider[] hitColiiders = 
            Physics.OverlapSphere
                (self.transform.position, self.EnemyData.ShieldBuffRadius);
        
        foreach (Collider hitCollider in hitColiiders)
        {
            if (hitCollider.CompareTag(nameof(ETags.Enemy)) &&
                hitCollider.TryGetComponent<EnemyController>(out EnemyController enemyController) &&
                !enemyController.EnemyData.IsShieldActive)
            {
                enemyController.ActivateShield();
                ShieldTrailEffect(self, enemyController);
            }
        }
    }

    private void ShieldTrailEffect(EnemyController from, EnemyController to)
    {
        Vector3 start = from.transform.position;
        Vector3 end = to.transform.position;

        TrailRenderer trail =
            VFXPoolManager.Instance
                .GetObject(EVFXType.EnemySupportTypeShieldTrail, start, quaternion.identity)
                .GetComponent<TrailRenderer>();

        float distance = Vector3.Distance(start, end);
        float duration = distance / 5f;

        from.StartCoroutineInEnemyState(MoveShieldTrail(trail, start, end, duration));
    }

    private IEnumerator MoveShieldTrail(TrailRenderer trail, Vector3 start, Vector3 end, float duration)
    {
        if (ReferenceEquals(trail, null))
        {
            yield break;
        }
        
        float time = 0f;
        while (time < duration)
        {
            trail.transform.position = Vector3.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.3f);
        VFXPoolManager.Instance.ReturnObject(trail.gameObject, EVFXType.EnemySupportTypeShieldTrail);
    }
}
