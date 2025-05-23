using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 20250523 박미르 수정함. UI 연동
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class PlayerArea : MonoBehaviour
{
    private SphereCollider _playerZone;
    private float _playerZoneRadius;
    public float PlayerZoneRadius => _playerZoneRadius;

    [SerializeField] private float _outBoundaryKillTime = 10f;
    private Coroutine _killCoroutine;

    public Action<float> OnOutBoundaryTimerUpdate;

    private void Start()
    {
        _playerZone = GetComponent<SphereCollider>();
        _playerZoneRadius = _playerZone.radius;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(nameof(ETags.Player))) return;

        InGame1_UI_PlayerOutBoundaryAlert.instance.OutBoundaryAlertOn();

        if (_killCoroutine == null)
            _killCoroutine = StartCoroutine(KillPlayerAfterDelay());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(nameof(ETags.Player))) return;

        InGame1_UI_PlayerOutBoundaryAlert.instance.OutBoundaryAlertOff();

        if (_killCoroutine != null)
        {
            StopCoroutine(_killCoroutine);
            _killCoroutine = null;
        }

        OnOutBoundaryTimerUpdate?.Invoke(0f);
    }

    private IEnumerator KillPlayerAfterDelay()
    {
        float elapsed = 0f;

        while (elapsed < _outBoundaryKillTime)
        {
            elapsed += Time.deltaTime;
            OnOutBoundaryTimerUpdate?.Invoke(elapsed);
            yield return null;
        }

        KillPlayer();
        _killCoroutine = null;
    }

    private void KillPlayer()
    {
        Debug.Log("Player killed due to boundary exit.");
    }
}