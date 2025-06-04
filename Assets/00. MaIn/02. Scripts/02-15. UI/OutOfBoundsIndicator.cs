using UnityEngine;

public class OutOfBoundsIndicator : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform indicator;
    [SerializeField] private Vector3 center = Vector3.zero;
    [SerializeField] private float radius = 50f;
    [SerializeField] private float distanceFromPlayer = 3f;

    private void Update()
    {
        float dist = Vector3.Distance(player.position, center);
        bool isOutOfBounds = dist > radius;

        indicator.gameObject.SetActive(isOutOfBounds);

        if (isOutOfBounds)
        {
            Vector3 dir = (center - player.position).normalized;

            // 인디케이터를 플레이어 앞쪽에 띄움
            indicator.position = player.position + dir * distanceFromPlayer;

            // 방향 맞춰 회전
            indicator.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}
