
using UnityEngine;

public class BearPatrolState : IState<BearController>
{
    private BearController _bear;
    private int _patrolPointIndex = -1;
    private bool _returningToSpawn = false;

    public void OnEnter(BearController bear)
    {
        _bear = bear;
        _bear.Animator.SetBool("IsWalking", true);
        Debug.Log("순찰 상태 진입");
        SetNewPatrolDestination();
    }

    public void OnUpdate()
    {
        // 플레이어 감지
        if (IsPlayerInDetectionRange())
        {
            _bear.StateMachine.SetState(typeof(BearChaseState));
            return;
        }

        // 목적지에 도착했는지 확인
        if (!_bear.Agent.pathPending && _bear.Agent.remainingDistance < 0.5f)
        {
            if (_returningToSpawn)
            {
                // 스폰 위치에 도착했으므로 Idle 상태로 변경
                _bear.StateMachine.SetState(typeof(BearIdleState));
            }
            else
            {
                // 순찰 지점에 도착했으므로 스폰 위치로 복귀
                ReturnToSpawnPoint();
            }
        }
    }

    public void OnExit()
    {
        _bear.Animator.SetBool("IsWalking", false);
        _bear.Agent.ResetPath();
        Debug.Log("순찰 상태 종료");
    }

    private void SetNewPatrolDestination()
    {
        _returningToSpawn = false;
        if (_bear.PatrolPoints.Length == 0)
        {
            // 순찰 지점이 없으면 바로 스폰 위치로 돌아가서 Idle
            ReturnToSpawnPoint();
            return;
        }

        _patrolPointIndex = Random.Range(0, _bear.PatrolPoints.Length);
        Transform targetPoint = _bear.PatrolPoints[_patrolPointIndex];
        _bear.Agent.SetDestination(targetPoint.position);
        Debug.Log($"{targetPoint.name}(으)로 순찰 시작");
    }

    private void ReturnToSpawnPoint()
    {
        _returningToSpawn = true;
        _bear.Agent.SetDestination(_bear.SpawnPosition);
        Debug.Log("스폰 위치로 복귀 시작");
    }

    private bool IsPlayerInDetectionRange()
    {
        if (_bear.Player == null) return false;
        float distanceToPlayer = Vector3.Distance(_bear.transform.position, _bear.Player.position);
        return distanceToPlayer <= _bear.detectionRange;
    }
}
